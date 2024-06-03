using Microsoft.AspNetCore.Mvc;

namespace TrainingManagementPortalAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;
    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
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
}