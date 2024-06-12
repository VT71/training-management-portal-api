using Microsoft.AspNetCore.Mvc;

namespace TrainingManagementPortalAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class TrainingsController : ControllerBase
{

    DataContextDapper _dapper;

    public TrainingsController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }


    [HttpPost("CreateTraining")]
    public IActionResult CreateTraining(Trainings trainings)
    {
        string sql = @"INSERT INTO TrainingDatabaseSchema.Trainings
                   ([Title], [Description], [Online], [Deadline], [Department], [Employee])
                   VALUES (@Title, @Description, @Online, @Deadline, @Department, @Employee)";

        var parameters = new
        {
            trainings.Title,
            trainings.Description,
            trainings.Online,
            trainings.Deadline,
            // trainings.Department,
            // trainings.Employee
        };

        if (_dapper.ExecuteSql(sql, parameters))
        {
            return Ok();
        }

        throw new Exception("Failed to update user");
    }
}