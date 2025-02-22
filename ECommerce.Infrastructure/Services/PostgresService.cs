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
        PostgresConfiguration _postgresConfiguration;

        public PostgresService(PostgresConfiguration postgresConfiguration)
        {
            _postgresConfiguration = postgresConfiguration;
            Connection = new NpgsqlConnection($@"
                HOST={postgresConfiguration.Host};
                PORT={postgresConfiguration.Port};
                USERNAME={postgresConfiguration.Username};
                PASSWORD={postgresConfiguration.Password};
                DATABASE={postgresConfiguration.Database};
            ");
            Connection.Open();
        }

        // Mostly used during integration tests
        public PostgresService(string connectionString)
        {
            _postgresConfiguration = null;
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
                    if (parameter.Key is not null)
                    {
                        cmd.Parameters.Add(new NpgsqlParameter(parameter.Key, parameter.Value));
                    }

                    else
                    {
                        cmd.Parameters.Add(new NpgsqlParameter { Value = parameter.Value });
                    }
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
                    object value = parameter.Value is null ? DBNull.Value : parameter.Value;

                    if (parameter.Key is not null)
                    {
                        cmd.Parameters.Add(new NpgsqlParameter(parameter.Key, value));
                    }

                    else
                    {
                        cmd.Parameters.Add(new NpgsqlParameter { Value = value });
                    }
                }
            }
            return await cmd.ExecuteScalarAsync();
        }
    }
}