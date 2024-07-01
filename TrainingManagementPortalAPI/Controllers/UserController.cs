using Microsoft.AspNetCore.Mvc;

namespace TrainingManagementPortalAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;
    private readonly IExternalApiService _externalApiService;

    public UserController(IConfiguration config, IExternalApiService externalApiService)
    {
        _dapper = new DataContextDapper(config);
        _externalApiService = externalApiService;
    }

    [HttpGet("GetUsers/{userId}")]
    // public IEnumerable<User> GetUsers()
    public User GetUser(string userId)
    {
        string sql = @"SELECT [UserId],
                        [FullName],
                        [Email],
                        [Role]
                    FROM TrainingDatabaseSchema.Users WHERE UserId = " + "'" + userId + "'";
        User user = _dapper.LoadDataSingle<User>(sql);

        return user;
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        var parameters = new { user.FullName, user.Email, user.UserId };

        string sql = @"UPDATE TrainingDatabaseSchema.Users
                SET [FullName] = @FullName, 
                [Email] = @Email
                WHERE UserId = @UserId";

        if (_dapper.ExecuteSql(sql, parameters))
        {
            return Ok();
        }

        throw new Exception("Failed to update user");
    }

    [HttpPost("CreateUser")]
    public async Task<User> CreateUser(User user)
    {
        string sql = @"INSERT INTO TrainingDatabaseSchema.Users
                    (UserId, FullName, Email, Role)
                    VALUES
                    (@UserId, @FullName, @Email, 'Employee');

                    SELECT [UserId],
                        [FullName],
                        [Email],
                        [Role]
                    FROM TrainingDatabaseSchema.Users
                    WHERE UserId = @UserId";

        var parameters = new { user.UserId, user.FullName, user.Email };

        User newUser = _dapper.LoadDataSingle<User>(sql, parameters);

        await _externalApiService.SendEmailToUser(user.Email, "New account created", "An account has been created for you on the Training Management Portal.\n\nYou should receive an email allowing you to reset your password and login.\n\nKind Regards,\nAdmin Team.");

        return newUser;
    }
}