using Microsoft.AspNetCore.Mvc;

namespace TrainingManagementPortalAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class ProgressController : ControllerBase
{

    DataContextDapper _dapper;

    public ProgressController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("GetAllProgressByEmployeeTraining")]
    public IEnumerable<SectionProgress> GetPercentageOfCompletedTrainingsByRange([FromQuery] int employeeId, int trainingId)
    {
        string sql = @"SELECT [P].[ProgressId],
                        [P].[SectionId],
                        [P].[EmployeeId],
                        [P].[Progress]
                    FROM TrainingDatabaseSchema.Sections AS S
                        JOIN TrainingDatabaseSchema.Progress AS P ON S.SectionId = P.SectionId
                    WHERE S.TrainingId = '" + trainingId + "' AND P.EmployeeId = '" + employeeId + "';";

        IEnumerable<SectionProgress> allProgress = _dapper.LoadData<SectionProgress>(sql);

        return allProgress;
    }
}
