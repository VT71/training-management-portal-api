using Microsoft.AspNetCore.Mvc;

namespace TrainingManagementPortalAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class ReportsController : ControllerBase
{

    DataContextDapper _dapper;

    public ReportsController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("GetPercentageOfCompletedTrainingsByRange")]
    public decimal GetPercentageOfCompletedTrainingsByRange([FromQuery] string startDate, string endDate)
    {
        var allParameters = new
        {
            StartDate = startDate,
            EndDate = endDate
        };

        string sql = @"SELECT COUNT(*)
                        FROM TrainingDatabaseSchema.Trainings AS T
                    WHERE T.Deadline >= @StartDate AND T.Deadline <= @EndDate";

        decimal totalTrainings = _dapper.LoadDataSingle<decimal>(sql, allParameters);

        var current = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

        var completedParameters = new
        {
            TodaysDate = current,
            StartDate = startDate,
            EndDate = endDate
        };

        sql = @"EXECUTE TrainingDatabaseSchema.GetCompletedTrainingsByRange @TodaysDate = @TodaysDate, @StartDate = @StartDate, @EndDate = @EndDate;";

        decimal totalCompletedTrainings = _dapper.LoadDataSingle<decimal>(sql, completedParameters);


        return Math.Round(totalCompletedTrainings / totalTrainings, 2);
    }
}
