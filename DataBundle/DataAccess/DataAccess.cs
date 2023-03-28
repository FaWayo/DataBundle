using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataBundle.DataAccess
{
    public class DataAccess : IDataAccess
    {
        private readonly IConfiguration _configuration;

        public DataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Database")
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionId));
            return await connection.QueryAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure
                );
        }

        public async Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "Database")
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionId));
            await connection.ExecuteAsync(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SaveDataWithRes<T>(string storedProcedure, T parameters, string connectionId = "Database")
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionId));
           var response = await connection.ExecuteAsync(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
            return response;
        }
    }

}
