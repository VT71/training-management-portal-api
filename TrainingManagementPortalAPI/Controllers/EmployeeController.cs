using Microsoft.AspNetCore.Mvc;

namespace TrainingManagementPortalAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    DataContextDapper _dapper;
    public EmployeeController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("GetEmployeesComplete")]
    public IEnumerable<EmployeeComplete> GetEmployeesComplete()
    {
        string sql = @"EXECUTE TrainingDatabaseSchema.getEmployeesComplete";

        IEnumerable<EmployeeComplete> employeesComplete = _dapper.LoadData<EmployeeComplete>(sql);

        return employeesComplete;
    }
}