using ECommerce.Infrastructure.Services;
using Testcontainers.PostgreSql;

namespace TestUtils;

public class TestDatabase
{
    public TestDatabase()
    {
        Task? initializationTask = CreateAsync();
        initializationTask.Wait();
    }

    private TestDatabase(PostgreSqlContainer container, PostgresService postgresService)
    {
        _postgresContainer = container;
        _postgresService = postgresService;
    }

    public PostgreSqlContainer _postgresContainer { get; set; }
    public PostgresService _postgresService { get; set; }

    public async Task CreateAsync()
    {
        string? solutionRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;
        if (solutionRoot is null) throw new FileLoadException("Couldn't find solution root path");
        string initSqlPath = Path.Combine(solutionRoot ?? "", "sql-init", "init.sql");

        _postgresContainer = new PostgreSqlBuilder()
            .WithDatabase("test")
            .WithUsername("test")
            .WithPassword("test")
            .WithResourceMapping(initSqlPath, "/docker-entrypoint-initdb.d")
            .Build();

        await _postgresContainer.StartAsync();


        _postgresService = new PostgresService(_postgresContainer.GetConnectionString());
    }

    public async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
    }
}