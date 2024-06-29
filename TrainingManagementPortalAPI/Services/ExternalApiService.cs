using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public interface IExternalApiService
{
    Task<string> SendEmailToUser(string toEmail, string subject, string body);
}

public class ExternalApiService : IExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public ExternalApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["EmailApiKey"];
        if (string.IsNullOrEmpty(_apiKey))
        {
            _apiKey = "";
        }
        _baseUrl = "https://api.sendgrid.com"; // Replace with your API base URL

        _httpClient.BaseAddress = new Uri(_baseUrl);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> SendEmailToUser(string toEmail, string subject, string body)
    {
        var emailData = new
        {
            personalizations = new[]
                {
                    new
                    {
                        to = new[]
                        {
                            new { email = toEmail }
                        }
                    }
                },
            from = new { email = "scanner_dim0k@icloud.com" },
            subject = subject,
            content = new[]
                {
                    new
                    {
                        type = "text/plain",
                        value = body
                    }
                }
        };

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(emailData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/v3/mail/send", content);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
