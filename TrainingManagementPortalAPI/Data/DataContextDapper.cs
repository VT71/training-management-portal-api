using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace TrainingManagementPortalAPI
{
    class DataContextDapper
    {
        private readonly IConfiguration _config;
        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<T> LoadData<T>(string sql, object? parameters = null)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sql, parameters);
        }
        public T LoadDataSingle<T>(string sql, object? parameters = null)
        {
            Console.WriteLine("CONN STRING SQLCONSTR: " + _config.GetConnectionString("SQLCONNSTR_DefaultConnection"));
            Console.WriteLine("CONN STRING: " + _config.GetConnectionString("DefaultConnection"));
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.QuerySingle<T>(sql, parameters);
        }

        public bool ExecuteSql(string sql, object? parameters = null)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql, parameters) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql);
        }

        internal T ExecuteScalar<T>(string sql, object value)
        {
            throw new NotImplementedException();
        }

        internal T LoadSingle<T>(string checkTrainerSql, object checkParameters)
        {
            throw new NotImplementedException();
        }
    }
}