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

    [HttpGet("GetEmployeeComplete/{employeeId}")]
    public EmployeeComplete GetEmployeeComplete(int employeeId)
    {
        string sql = @"EXECUTE TrainingDatabaseSchema.getEmployeeComplete @EmployeeId = '" + employeeId + "'";

        EmployeeComplete employeeComplete = _dapper.LoadDataSingle<EmployeeComplete>(sql);

        return employeeComplete;
    }

    [HttpGet("GetEmployees")]
    public IEnumerable<Employee> GetEmployees()
    {
        string sql = @"SELECT [EmployeeId],
                        [Trainer],
                        [UserId],
                        [DepartmentId]
                    FROM
                        TrainingDatabaseSchema.Employees
                    GO";

        IEnumerable<Employee> employees = _dapper.LoadData<Employee>(sql);

        return employees;
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

    [HttpPut("EditEmployee")]
    public IActionResult EditUser(Employee employee)
    {
        var parameters = new { employee.EmployeeId, employee.Trainer, employee.DepartmentId };

        string sql = @"UPDATE TrainingDatabaseSchema.Employees
                SET [Trainer] = @Trainer, 
                [DepartmentId] = @DepartmentId
                WHERE EmployeeId = @EmployeeId";

        if (_dapper.ExecuteSql(sql, parameters))
        {
            return Ok();
        }

        throw new Exception("Failed to update user");
    }
}

