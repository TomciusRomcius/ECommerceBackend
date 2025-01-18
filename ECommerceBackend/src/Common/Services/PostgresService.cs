using Npgsql;

namespace ECommerce.Common.Services
{
    public class QueryParameter
    {
        public string Key { get; set; }
        public object Value { get; set; }

        public QueryParameter(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
    public interface IPostgresService
    {
        public Task<List<object>> ExecuteReaderAsync(string query, QueryParameter[] parameters);
        public Task<object?> ExecuteScalarAsync(string query, QueryParameter[] parameters);
    }

    public class PostgresService : IPostgresService
    {
        public NpgsqlConnection Connection { get; }

        public PostgresService(string connectionString)
        {
            Connection = new NpgsqlConnection(connectionString);
            Connection.Open();
        }

        public async Task<List<object>> ExecuteReaderAsync(string query, QueryParameter[] parameters)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(query, Connection);
            foreach (var parameter in parameters)
            {
                cmd.Parameters.Add(new NpgsqlParameter(parameter.Key, parameter.Value));
            }
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            List<object> result = new List<object>();

            while (await reader.ReadAsync())
            {
                for (int ordinal = 0; ordinal < reader.FieldCount; ordinal++)
                {
                    result.Add(reader.GetValue(ordinal));
                }
            }

            return result;
        }

        public async Task<object?> ExecuteScalarAsync(string query, QueryParameter[] parameters)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(query, Connection);
            foreach (var parameter in parameters)
            {
                cmd.Parameters.Add(new NpgsqlParameter(parameter.Key, parameter.Value));
            }
            return await cmd.ExecuteScalarAsync();
        }
    }
}