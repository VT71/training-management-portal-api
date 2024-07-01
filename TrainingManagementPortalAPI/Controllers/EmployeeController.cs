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

    [HttpGet("Test")]
    public async Task<IActionResult> Test()
    {
        try
        {
            var externalData = await _externalApiService.SendEmailToUser("tomav98@gmail.com", "New account created", "An account has been created for you on the Training Management Portal.\nYou should receive an email allowing you to reset your password and login.\nThank you.");

            return Ok(externalData);
        }
        catch (Exception e)
        {
            return StatusCode(500, "An unexpected error occurred");
        }
    }
}

