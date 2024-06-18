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
                   ([Title], [Description], [Online], [Deadline], [forDepartments] , [forEmployees])
                   VALUES (@Title, @Description, @Online, @Deadline), @forDepartments, @forEmployees";

        var parameters = new
        {
            trainings.Title,
            trainings.Description,
            trainings.Online,
            trainings.Deadline,
            trainings.forDepartments,
            trainings.forEmployees,
        };

        if (_dapper.ExecuteSql(sql, parameters))
        {
            return Ok();
        }

        throw new Exception("Failed to update user");
    }

    [HttpGet("GetTrainings")]

    public IEnumerable<Trainings> GetTrainings()
    {
        string sql = @"SELECT [TrainingId],
                    [Title],
                    [Description],
                    [Online],
                    [Deadline],
                    [ForEmployees],
                    [ForDepartments]

    FROM TrainingDatabaseSchema.Trainings
";

        var trainings = _dapper.LoadData<Trainings>(sql);

        return trainings;

    }
}
