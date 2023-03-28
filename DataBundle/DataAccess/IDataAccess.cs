namespace DataBundle.DataAccess
{
    public interface IDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Database");
        Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "Database");
        Task<int> SaveDataWithRes<T>(string storedProcedure, T parameters, string connectionId = "Database");
    }
}