using ECommerce.DataAccess.Utils;
using Npgsql;

namespace ECommerce.DataAccess.Services
{
    public interface IPostgresService
    {
        /// <summary>
        ///     Execute Postgres query
        /// </summary>
        /// <returns>
        ///     Returns a list of rows
        /// </returns>
        public Task<List<Dictionary<string, object>>> ExecuteAsync(string query, QueryParameter[]? parameters = null);
        public Task<object?> ExecuteScalarAsync(string query, QueryParameter[]? parameters = null);
    }

    public class PostgresService : IPostgresService
    {
        public NpgsqlConnection Connection { get; }

        public PostgresService(string connectionString)
        {
            Connection = new NpgsqlConnection(connectionString);
            Connection.Open();
        }

        public async Task<List<Dictionary<string, object>>> ExecuteAsync(string query, QueryParameter[]? parameters = null)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(query, Connection);

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.Add(new NpgsqlParameter(parameter.Key, parameter.Value));
                }
            }

            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int ordinal = 0; ordinal < reader.FieldCount; ordinal++)
                {
                    row[reader.GetName(ordinal)] = reader.GetValue(ordinal);
                }

                result.Add(row);
            }

            return result;
        }

        public async Task<object?> ExecuteScalarAsync(string query, QueryParameter[]? parameters = null)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(query, Connection);

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.Add(new NpgsqlParameter(parameter.Key, parameter.Value));
                }
            }

            return await cmd.ExecuteScalarAsync();
        }
    }
}