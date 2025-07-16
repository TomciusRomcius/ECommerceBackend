
using ECommerce.Persistence.src;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using TestUtils;

namespace ECommerce.Application.Tests.Utils
{
    public abstract class DbContextWithDependencyInjection : IAsyncDisposable
    {
        protected readonly IServiceProvider ServiceProvider;
        private readonly IServiceCollection _services;
        protected readonly PostgreSqlContainer PostgresContainer;
        protected readonly DatabaseContext DbContext;

        protected DbContextWithDependencyInjection()
        {
            _services = new ServiceCollection();
            _services.AddLogging();
            // Creates database instance and registers DbContext
            PostgresContainer = TestDbUtils.CreateDbCtxAndDb(_services);
            PreServiceProviderCreation(_services);
            ServiceProvider = _services.BuildServiceProvider();
            DbContext = ServiceProvider.GetRequiredService<DatabaseContext>();
            DbContext.Database.Migrate();
        }

        protected abstract void PreServiceProviderCreation(IServiceCollection services);

        public async ValueTask DisposeAsync()
        {
            await PostgresContainer.DisposeAsync();
        }
    }
}
