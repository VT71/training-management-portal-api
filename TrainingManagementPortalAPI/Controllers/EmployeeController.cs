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

    [HttpPost("CreateEmployee")]
    public Employee CreateEmployee(Employee employee)
    {

        var parameters = new { employee.Trainer, employee.UserId, employee.DepartmentId };

        string sql = @"INSERT INTO TrainingDatabaseSchema.Employees
                        (Trainer, UserId, DepartmentId)
                    VALUES
                        (@Trainer, @UserId, @DepartmentId);
                    
                    SELECT [EmployeeId],
                        [Trainer],
                        [UserId],
                        [DepartmentId]
                    FROM
                        TrainingDatabaseSchema.Employees
                    WHERE UserId = @UserId;";

        Employee newEmployee = _dapper.LoadDataSingle<Employee>(sql, parameters);

        return newEmployee;
    }
}

