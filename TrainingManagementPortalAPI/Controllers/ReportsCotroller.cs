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

        sql = @"EXECUTE TrainingDatabaseSchema.GetCompletedTrainingsByRange 
                @TodaysDate = @TodaysDate, 
                @StartDate = @StartDate, 
                @EndDate = @EndDate;";

        decimal totalCompletedTrainings = _dapper.LoadDataSingle<decimal>(sql, completedParameters);


        return Math.Round(totalCompletedTrainings / totalTrainings, 2);
    }

    [HttpGet("GetDepartmentsProgress")]
    public IEnumerable<DepartmentProgress> GetDepartmentsCompletionRates([FromQuery] string startDate, string endDate)
    {
        var current = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

        var parameters = new
        {
            TodaysDate = current,
            StartDate = startDate,
            EndDate = endDate
        };

        string sql = @"EXECUTE TrainingDatabaseSchema.GetDepartmentsCompletionRates 
                    @TodaysDate = @TodaysDate, 
                    @StartDate = @StartDate, 
                    @EndDate = @EndDate";

        IEnumerable<DepartmentProgress> departmentsProgresses = _dapper.LoadData<DepartmentProgress>(sql, parameters);

        return departmentsProgresses;
    }

    [HttpGet("GetTotalTrainingsByType")]
    public IEnumerable<TrainingTypeStat> GetTotalTrainingsByType([FromQuery] string startDate, string endDate)
    {
        var parameters = new
        {
            StartDate = startDate,
            EndDate = endDate
        };

        string sql = @"EXECUTE TrainingDatabaseSchema.GetTotalTrainingsByType 
                    @StartDate = @StartDate, 
                    @EndDate = @EndDate";

        IEnumerable<TrainingTypeStat> departmentsProgresses = _dapper.LoadData<TrainingTypeStat>(sql, parameters);

        return departmentsProgresses;
    }
}
