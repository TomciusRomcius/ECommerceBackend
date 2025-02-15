using ECommerce.DataAccess.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Testcontainers.PostgreSql;

namespace DataAccess.Test.Integration.Utils
{
    public class TestContainerPostgresServiceWrapper
    {
        public PostgreSqlContainer _postgresContainer { get; set; }
        public PostgresService _postgresService { get; set; }

        private TestContainerPostgresServiceWrapper(PostgreSqlContainer container, PostgresService postgresService)
        {
            _postgresContainer = container;
            _postgresService = postgresService;
        }

        public static async Task<TestContainerPostgresServiceWrapper> CreateAsync()
        {
            string? solutionRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;
            if (solutionRoot is null)
            {
                throw new FileLoadException("Couldn't find solution root path");
            }
            var initSqlPath = Path.Combine(solutionRoot ?? "", "sql-init", "init.sql");
            PostgreSqlContainer container = new PostgreSqlBuilder()
            .WithDatabase("test")
            .WithUsername("test")
            .WithPassword("test")
            .WithResourceMapping(initSqlPath, "/docker-entrypoint-initdb.d")
            .Build();

            await container.StartAsync();

            Mock<ILogger> logger = new Mock<ILogger>();

            var postgresService = new PostgresService(container.GetConnectionString());
            return new TestContainerPostgresServiceWrapper(container, postgresService);
        }

        public async Task DisposeAsync()
        {
            await _postgresContainer.DisposeAsync();
        }
    }
}