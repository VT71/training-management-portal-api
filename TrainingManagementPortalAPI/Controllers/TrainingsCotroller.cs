using Microsoft.AspNetCore.Mvc;

namespace TrainingManagementPortalAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class TrainingsController : ControllerBase
{

    DataContextDapper _dapper;

    public TrainingsController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("GetTrainings")]

    public IEnumerable<Trainings> GetTrainings()
    {
        string sql = @"SELECT [TrainingId],
                    [Title],
                    [Description],
                    [Individual],
                    [Adress],
                    [Deadline],
                    [Trainer],
                    [ForEmployees],
                    [ForDepartments]
    FROM TrainingDatabaseSchema.Trainings
";

        var trainings = _dapper.LoadData<Trainings>(sql);

        return trainings;

    }

    [HttpPost("CreateTraining")]
    public IActionResult CreateTraining(TrainingsComplete trainings)
    {
        Console.WriteLine(trainings);
        string sql = @"EXECUTE TrainingDatabaseSchema.CreateTraining 
        @Title = @Title, 
        @Description = @Description, 
        @Individual = @Individual, 
        @Adress = @Adress, 
        @Deadline = @Deadline, 
        @Trainer = @Trainer, 
        @ForDepartments = @ForDepartments, 
        @ForEmployees = @ForEmployees";

        var parameters = new
        {
            trainings.Title,
            trainings.Description,
            trainings.Individual,
            trainings.Adress,
            trainings.Deadline,
            trainings.Trainer,
            trainings.ForDepartments,
            trainings.ForEmployees
        };

        Trainings newTraining = _dapper.LoadDataSingle<Trainings>(sql, parameters);

        foreach (var employee in trainings.Employees)
        {
            string sqlConnectEmployee = @"EXECUTE TrainingDatabaseSchema.connectEmployeeWithTraining 
            @EmployeeId = @EmployeeId,
            @TrainingId = @TrainingId";

            var parametersConnectEmployee = new
            {
                employee.EmployeeId,
                newTraining.TrainingId
            };

            _dapper.ExecuteSql(sqlConnectEmployee, parametersConnectEmployee);
        }

        // 3. Conectare departamente cu trainingul (apel procedură stocată connectDepartmentWithTraining)
        foreach (var department in trainings.Departments)
        {
            string sqlConnectDepartment = @"EXECUTE TrainingDatabaseSchema.connectDepartmentWithTraining 
                                        @DepartmentId = @DepartmentId,
                                        @TrainingId =  @TrainingId";

            var parametersConnectDepartment = new
            {
                department.DepartmentId,
                newTraining.TrainingId
            };

            _dapper.ExecuteSql(sqlConnectDepartment, parametersConnectDepartment);
        }

        return Ok();

    }

    [HttpGet("GetTraining/{trainingId}")]
    public TrainingsComplete GetTraining(int trainingId)
    {
        string sql = @"EXECUTE TrainingDatabaseSchema.GetCompleteTraining @TrainingId = '" + trainingId + "'";
        var parameters = new { TrainingId = trainingId };


        var training = _dapper.LoadDataSingle<TrainingsComplete>(sql, parameters);

        if (training != null)
        {
            if (training.ForEmployees == 1)
            {
                sql = @"EXECUTE TrainingDatabaseSchema.getEmployeesCompleteByTraining @TrainingId =  '" + trainingId + "'";
                var employees = _dapper.LoadData<EmployeeComplete>(sql);

                if (employees != null)
                {
                    training.Employees = employees;
                }
            }

            if (training.ForDepartments == 1)
            {
                sql = @"EXECUTE TrainingDatabaseSchema.getDepartmentsByTraining @TrainingId = '" + trainingId + "'";
                var departments = _dapper.LoadData<Department>(sql);

                if (departments != null)
                {
                    training.Departments = departments;
                }
            }
        }
        return training;

    }

    [HttpPut("UpdateTraining/{trainingId}")]
    public IActionResult UpdateTraining(int trainingId, TrainingsComplete trainings)
    {
        string sql = @"EXECUTE TrainingDatabaseSchema.UpdateTraining 
        @TrainingId = @TrainingId,
        @Title = @Title, 
        @Description = @Description, 
        @Individual = @Individual, 
        @Adress = @Adress, 
        @Deadline = @Deadline, 
        @Trainer = @Trainer, 
        @ForDepartments = @ForDepartments, 
        @ForEmployees = @ForEmployees";

        var parameters = new
        {
            TrainingId = trainingId,
            trainings.Title,
            trainings.Description,
            trainings.Individual,
            trainings.Adress,
            trainings.Deadline,
            trainings.Trainer,
            trainings.ForDepartments,
            trainings.ForEmployees,
        };

        if (_dapper.ExecuteSql(sql, parameters))
        {
            return Ok();
        }

        throw new Exception("Failed to update training");
    }


    [HttpDelete("DeleteTraining/{trainingId}")]
    public IActionResult DeleteTraining(int trainingId)
    {
        string sql = @"EXECUTE TrainingDatabaseSchema.DeleteTraining @TrainingId = '" + trainingId + "'";

        var parameters = new { TrainingId = trainingId };

        if (_dapper.ExecuteSql(sql, parameters))
        {
            return Ok();
        }

        throw new Exception("Failed to delete training");
    }

    [HttpGet("GetMissedTrainingsByEmployee/{employeeId}")]
    public IEnumerable<Trainings> GetMissedTrainingsByEmployee(int employeeId)
    {
        var current = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

        string sql = @"EXECUTE TrainingDatabaseSchema.GetMissedTrainingsByEmployee @EmployeeId = '" + employeeId + "', @TodayDateTime = '" + current + "'";

        IEnumerable<Trainings> missedTrainings = _dapper.LoadData<Trainings>(sql);

        return missedTrainings;
    }

    [HttpGet("GetCompletedTrainingsByEmployee/{employeeId}")]
    public IEnumerable<Trainings> GetCompletedTrainingsByEmployee(int employeeId)
    {
        var current = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

        string sql = @"EXECUTE TrainingDatabaseSchema.GetCompletedTrainingsByEmployee @EmployeeId = '" + employeeId + "', @TodayDateTime = '" + current + "'";

        IEnumerable<Trainings> completedTrainings = _dapper.LoadData<Trainings>(sql);

        return completedTrainings;
    }

    [HttpGet("GetUpcomingTrainingsByEmployee/{employeeId}")]
    public IEnumerable<Trainings> GetUpcomingTrainingsByEmployee(int employeeId)
    {
        var current = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

        string sql = @"EXECUTE TrainingDatabaseSchema.GetUpcomingTrainingsByEmployee @EmployeeId = '" + employeeId + "', @TodayDateTime = '" + current + "'";

        IEnumerable<Trainings> upcomingTrainings = _dapper.LoadData<Trainings>(sql);

        return upcomingTrainings;
    }

    [HttpGet("GetInProgressTrainingsByEmployee/{employeeId}")]
    public IEnumerable<Trainings> GetInProgressTrainingsByEmployee(int employeeId)
    {
        var current = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

        string sql = @"EXECUTE TrainingDatabaseSchema.GetInProgressTrainingsByEmployee @EmployeeId = '" + employeeId + "', @TodayDateTime = '" + current + "'";

        IEnumerable<Trainings> upcomingTrainings = _dapper.LoadData<Trainings>(sql);

        return upcomingTrainings;
    }

    [HttpGet("GetMissedTrainings")]
    public IEnumerable<Trainings> GetMissedTrainings()
    {
        var current = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

        string sql = @"EXECUTE TrainingDatabaseSchema.GetMissedTrainings @TodayDateTime = '" + current + "'";

        IEnumerable<Trainings> missedTrainings = _dapper.LoadData<Trainings>(sql);

        return missedTrainings;
    }

    [HttpGet("GetUpcomingTrainings")]
    public IEnumerable<Trainings> GetUpcomingTrainings()
    {
        var current = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        Console.WriteLine(current);

        string sql = @"EXECUTE TrainingDatabaseSchema.GetUpcomingTrainings @TodayDateTime = '" + current + "'";

        IEnumerable<Trainings> upcomingTrainings = _dapper.LoadData<Trainings>(sql);

        return upcomingTrainings;
    }

    [HttpGet("GetPercentageOfCompletedTrainingsByRange")]
    public decimal GetPercentageOfCompletedTrainingsByRange(string startDate, string endDate)
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
