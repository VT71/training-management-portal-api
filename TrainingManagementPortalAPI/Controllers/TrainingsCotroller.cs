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


    // [HttpGet("GetTraining/{trainingId}")]
    // // public IEnumerable<User> GetUsers()
    // public Trainings GetTraining(int trainingId)
    // {
    // Training Model a fost schimbat. Actualizeaza codul.
    //     string sql = @"SELECT [TrainingId],
    //                     [Title],
    //                     [Description],
    //                     [Online],
    //                     [Deadline],
    //                     [Departament],
    //                     [Employee]
    //                 FROM TrainingDatabaseSchema.Trainings WHERE TrainingId = " + trainingId.ToString();
    //     Trainings training = _dapper.LoadDataSingle<Trainings>(sql);

    //     return training;
    // }


    [HttpPut("UpdateTraining")]
    public IActionResult UdpateTraining(int trainingId, Trainings trainings)
    {
        // Training Model a fost schimbat. Actualizeaza codul.
        // string sql = @"UPDATE TrainingDatabaseSchema.Trainings
        //            SET [Title] = '" + trainings.Title +
        //             " ',[Description] =  '" + trainings.Description +
        //             " ',[Online] = ' " + trainings.Description +
        //             " ',[Deadline] = '" + trainings.Deadline +
        //             " ' [Departament] = '" + trainings.Deadline +
        //             " ' [Employee] = '" + trainings.Employee +
        //             " ' WHERE TrainingId = " + trainingId;



        // if (_dapper.ExecuteSql(sql))
        // {
            return Ok();
        // }

        // throw new Exception("Failed to update user");
    }
}