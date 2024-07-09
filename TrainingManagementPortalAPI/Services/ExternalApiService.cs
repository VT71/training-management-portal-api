using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using TrainingManagementPortalAPI;
public interface IExternalApiService
{
    Task<string> SendEmailToUser(string toEmail, string subject, string body);
    Task NotifyTrainingParticipants(Trainings newTraining, IEnumerable<EmployeeComplete> employees, string type);
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
        Console.WriteLine("API KEY: " + _apiKey);
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

    public async Task NotifyTrainingParticipants(Trainings newTraining, IEnumerable<EmployeeComplete> employees, string type)
    {
        string message = "";
        if (type == "trainer" && employees.Count() > 0)
        {
            foreach (var employee in employees)
            {
                message = $"Dear {employee.FullName},\n\nYou have a new training to conduct.\n\nTraining Name: {newTraining.Title}\nTraining Type: Workshop\nTraining Link/Address: {newTraining.Adress}\nTraining Date: {newTraining.Deadline.Day + "/" + newTraining.Deadline.Month + "/" + newTraining.Deadline.Year}\n\nKind Regards,\nAdmin Team.";
                await SendEmailToUser(employee.Email, "New Training Assigned", message);
            }

        }

        if (type == "employee" && employees.Count() > 0)
        {
            foreach (var employee in employees)
            {
                if (newTraining.Individual == 0)
                {
                    message = $"Dear {employee.FullName},\n\nYou have a new training to complete.\n\nTraining Name: {newTraining.Title}\nTraining Type: Workshop\nTraining Link/Address: {newTraining.Adress}\nTraining Date: {newTraining.Deadline.Day + "/" + newTraining.Deadline.Month + "/" + newTraining.Deadline.Year}\n\nKind Regards,\nAdmin Team.";
                }
                else
                {
                    message = $"Dear {employee.FullName},\n\nYou have a new training to complete.\n\nTraining Name: {newTraining.Title}\nTraining Type: Individual\nTraining Deadline: {newTraining.Deadline.Day + "/" + newTraining.Deadline.Month + "/" + newTraining.Deadline.Year}\n\nKind Regards,\nAdmin Team.";
                }
                await SendEmailToUser(employee.Email, "New Training Assigned", message);
            }

        }

    }
}
