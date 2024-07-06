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

    [HttpGet("GetAllProgressByUserTraining")]
    public IEnumerable<SectionProgress> GetAllProgressByUserTraining([FromQuery] string userId, int trainingId)
    {
        string sql = "";

        sql = @"SELECT E.EmployeeId FROM TrainingDatabaseSchema.Employees AS E 
                                JOIN TrainingDatabaseSchema.Users AS U ON E.UserId = U.UserId 
                            WHERE U.UserId = '" + userId + "'";

        int userEmployeeId = _dapper.LoadDataSingle<int>(sql);

        sql = @"SELECT [P].[ProgressId],
                        [P].[SectionId],
                        [P].[EmployeeId],
                        [P].[Progress]
                    FROM TrainingDatabaseSchema.Sections AS S
                        JOIN TrainingDatabaseSchema.Progress AS P ON S.SectionId = P.SectionId
                    WHERE S.TrainingId = '" + trainingId + "' AND P.EmployeeId = '" + userEmployeeId + "';";

        IEnumerable<SectionProgress> allProgress = _dapper.LoadData<SectionProgress>(sql);

        return allProgress;
    }

    [HttpPost("UpdateSectionProgress")]
    public IActionResult UpdateSectionProgress(int sectionId, string userId, int progress)
    {
        string sql = "";

        sql = @"SELECT E.EmployeeId FROM TrainingDatabaseSchema.Employees AS E 
                                JOIN TrainingDatabaseSchema.Users AS U ON E.UserId = U.UserId 
                            WHERE U.UserId = '" + userId + "'";

        int userEmployeeId = _dapper.LoadDataSingle<int>(sql);

        if (userEmployeeId > 0)
        {
            var parameters = new
            {
                SectionId = sectionId,
                EmployeeId = userEmployeeId,
                Progress = progress
            };

            sql = @"MERGE TrainingDatabaseSchema.Progress AS target
                    USING (SELECT @SectionId AS SectionId, @EmployeeId AS EmployeeId) AS source
                        ON (target.SectionId = source.SectionId AND target.EmployeeId = source.EmployeeId)
                    WHEN MATCHED THEN
                        UPDATE SET Progress = @Progress
                    WHEN NOT MATCHED BY TARGET THEN
                        INSERT (SectionId, EmployeeId, Progress)
                        VALUES (@SectionId, @EmployeeId, 0);";

            if (_dapper.ExecuteSql(sql, parameters) == true)
            {
                return Ok("Progress updated successfully");
            }
            else
            {
                return BadRequest("Progress not updated");
            }
        }
        else
        {
            return BadRequest("User not found");
        }
    }
}
