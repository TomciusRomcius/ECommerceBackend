using ECommerce.Application.src.Utils;
using ECommerce.Infrastructure.src.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace ECommerce.Infrastructure.src.Services;

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
    private readonly PostgresConfiguration _postgresConfiguration;
    private readonly ILogger<PostgresService> _logger;

    public PostgresService(IOptions<PostgresConfiguration> postgresConfiguration, ILogger<PostgresService> logger)
    {

        _postgresConfiguration = postgresConfiguration.Value;
        _logger = logger;
        _logger.LogTrace("Entering PostgresService constructor");
        _logger.LogInformation("Connecting to {}:{}", _postgresConfiguration.Host, _postgresConfiguration.Port);

        string connectionString = $@"
            HOST={_postgresConfiguration.Host};
            PORT={_postgresConfiguration.Port};
            USERNAME={_postgresConfiguration.Username};
            PASSWORD={_postgresConfiguration.Password};
            DATABASE={_postgresConfiguration.Database};
        ";

        _logger.LogDebug("Connecting to PostgreSQL. Connection string: {}", connectionString);

        Connection = new NpgsqlConnection(connectionString);
        Connection.Open();
    }

    // Mostly used during integration tests
    public PostgresService(string connectionString)
    {
        _postgresConfiguration = null!;
        Connection = new NpgsqlConnection(connectionString);
        Connection.Open();
        _logger = LoggerFactory.Create(options => options.SetMinimumLevel(LogLevel.Debug)).CreateLogger<PostgresService>();
    }

    public async Task<List<Dictionary<string, object>>> ExecuteAsync(string query, QueryParameter[]? parameters = null)
    {
        _logger.LogTrace("ExecuteAsync called");
        _logger.LogDebug("Executing query via ExecuteAsync. Query: {} Parameters: {}", query, parameters);
        var cmd = new NpgsqlCommand(query, Connection);

        if (parameters != null)
            foreach (QueryParameter parameter in parameters)
                if (parameter.Key is not null)
                    cmd.Parameters.Add(new NpgsqlParameter(parameter.Key, parameter.Value));

                else
                    cmd.Parameters.Add(new NpgsqlParameter { Value = parameter.Value });

        await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

        var result = new List<Dictionary<string, object>>();

        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object>();
            for (var ordinal = 0; ordinal < reader.FieldCount; ordinal++)
                row[reader.GetName(ordinal)] = reader.GetValue(ordinal);

            result.Add(row);
        }

        return result;
    }

    public async Task<object?> ExecuteScalarAsync(string query, QueryParameter[]? parameters = null)
    {
        _logger.LogTrace("ExecuteScalarAsync called");
        _logger.LogDebug("Executing query via ExecuteScalarAsync. Query: {} Parameters: {}", query, parameters);

        var cmd = new NpgsqlCommand(query, Connection);

        if (parameters != null)
            foreach (QueryParameter parameter in parameters)
            {
                object value = parameter.Value is null ? DBNull.Value : parameter.Value;

                if (parameter.Key is not null)
                    cmd.Parameters.Add(new NpgsqlParameter(parameter.Key, value));

                else
                    cmd.Parameters.Add(new NpgsqlParameter { Value = value });
            }

        return await cmd.ExecuteScalarAsync();
    }
}