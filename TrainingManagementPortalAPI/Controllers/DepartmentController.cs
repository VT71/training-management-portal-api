using Microsoft.AspNetCore.Mvc;

namespace TrainingManagementPortalAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController : ControllerBase
{
    DataContextDapper _dapper;
    public DepartmentController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("GetDepartments")]
    public IEnumerable<Department> GetDepartments()
    {
        string sql = @"SELECT [DepartmentId],
                        [DepartmentName]
                    FROM TrainingDatabaseSchema.Departments
                    GO";
        IEnumerable<Department> departments = _dapper.LoadData<Department>(sql);

        return departments;
    }
}