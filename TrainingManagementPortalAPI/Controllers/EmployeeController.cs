using Microsoft.AspNetCore.Mvc;

namespace TrainingManagementPortalAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    DataContextDapper _dapper;
    private readonly IExternalApiService _externalApiService;
    public EmployeeController(IConfiguration config, IExternalApiService externalApiService)
    {
        _dapper = new DataContextDapper(config);
        _externalApiService = externalApiService;
    }

    [HttpGet("GetEmployeesComplete")]
    public IEnumerable<EmployeeComplete> GetEmployeesComplete()
    {
        string sql = @"EXECUTE TrainingDatabaseSchema.getEmployeesComplete";

        IEnumerable<EmployeeComplete> employeesComplete = _dapper.LoadData<EmployeeComplete>(sql);

        return employeesComplete;
    }

    [HttpGet("GetEmployeeComplete")]
    public EmployeeComplete GetEmployeeComplete(int employeeId, string? userId)
    {
        string sql = "";

        if (userId != null) {
            sql = @"SELECT E.EmployeeId FROM TrainingDatabaseSchema.Employees AS E 
                                JOIN TrainingDatabaseSchema.Users AS U ON E.UserId = U.UserId 
                            WHERE U.UserId = '" + userId + "'";

            int userEmployeeId = _dapper.LoadDataSingle<int>(sql);

            sql = @"EXECUTE TrainingDatabaseSchema.getEmployeeComplete @EmployeeId = '" + userEmployeeId + "'";

            EmployeeComplete employeeComplete = _dapper.LoadDataSingle<EmployeeComplete>(sql);

            return employeeComplete;
        } else {
            sql = @"EXECUTE TrainingDatabaseSchema.getEmployeeComplete @EmployeeId = '" + employeeId + "'";

            EmployeeComplete employeeComplete = _dapper.LoadDataSingle<EmployeeComplete>(sql);

            return employeeComplete;
        }
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

    [HttpGet("GetTrainersComplete")]
    public IEnumerable<EmployeeComplete> GetTrainersComplete()
    {
        string sql = @"EXECUTE TrainingDatabaseSchema.getTrainersComplete";

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

    [HttpPut("EditEmployee")]
    public IActionResult EditEmployee(Employee employee)
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

