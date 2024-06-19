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

    [HttpGet("GetTrainings")]

    public IEnumerable<Trainings> GetTrainings()
    {
        string sql = @"SELECT [TrainingId],
                    [Title],
                    [Description],
                    [Individual],
                    [Adress],
                    [Deadline],
                    [Trainer],
                    [ForEmployees],
                    [ForDepartments]

    FROM TrainingDatabaseSchema.Trainings
";

        var trainings = _dapper.LoadData<Trainings>(sql);

        return trainings;

    }

    [HttpGet("GetTraining/{trainingId}")]
    public IActionResult GetTraining(int trainingId)
    {
        string sql = @"SELECT [TrainingId],
                    [Title],
                    [Description],
                    [Individual],
                    [Adress],
                    [Deadline],
                    [Trainer],
                    [ForEmployees],
                    [ForDepartments]
    FROM TrainingDatabaseSchema.Trainings
    WHERE [TrainingId] = @TrainingId
";

        var parameters = new { TrainingId = trainingId };

        var training = _dapper.LoadDataSingle<Trainings>(sql, parameters);

        if (training != null)
        {
            return Ok(training); // return the training object, not just the ID
        }

        return NotFound();
    }


    [HttpPut("UpdateTraining/{trainingId}")]
    public IActionResult UpdateTraining(int trainingId, Trainings trainings)
    {
        string sql = @"
        UPDATE TrainingDatabaseSchema.Trainings
        SET [Title] = @Title,
            [Description] = @Description,
            [Individual] = @Individual,
            [Adress], = @Adress,
            [Deadline] = @Deadline,
            [Trainer] = @Trainer,
            [ForDepartments] = @ForDepartments,
            [ForEmployees] = @ForEmployees
        WHERE [TrainingId] = @TrainingId
    ";

        var parameters = new
        {
            TrainingId = trainingId,
            trainings.Title,
            trainings.Description,
            trainings.Individual,
            trainings.Adress,
            trainings.Deadline,
            trainings.Trainer,
            trainings.ForDepartments,
            trainings.ForEmployees
        };

        if (_dapper.ExecuteSql(sql, parameters))
        {
            return Ok();
        }

        throw new Exception("Failed to update training");
    }



    [HttpPost("CreateTraining")]
    public IActionResult CreateTraining(Trainings trainings)
    {
        string sql = @"
        INSERT INTO TrainingDatabaseSchema.Trainings
        ([Title], [Description], [Individual], [Adress], [Deadline],[Trainer], [ForDepartments], [ForEmployees])
        VALUES (@Title, @Description, @Individual, @Adress, @Deadline, @Trainer, @ForDepartments, @ForEmployees);
        ";

        var parameters = new
        {
            trainings.Title,
            trainings.Description,
            trainings.Individual,
            trainings.Adress,
            trainings.Deadline,
            trainings.Trainer,
            trainings.ForDepartments,
            trainings.ForEmployees
        };

        if (_dapper.ExecuteSql(sql, parameters))

        {

            return Ok();
        }

        throw new Exception("Failed to update user");
    }


    [HttpDelete("DeleteTraining/{trainingId}")]
    public IActionResult DeleteTraining(int trainingId)
    {
        string sql = @"EXECUTE TrainingDatabaseSchema.DeleteTraining @TrainingId = '" + trainingId + "'";

        var parameters = new { TrainingId = trainingId };

        if (_dapper.ExecuteSql(sql, parameters))
        {
            return Ok();
        }

        throw new Exception("Failed to delete training");
    }
}
