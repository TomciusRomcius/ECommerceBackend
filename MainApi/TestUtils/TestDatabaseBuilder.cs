using ECommerce.Persistence;
using ECommerce.Persistence.src;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace TestUtils;

public static class TestDbUtils
{
    /// <summary>
    /// Creates database container, database context and applies migrations
    /// </summary>
    public static PostgreSqlContainer CreateDbCtxAndDb(IServiceCollection services)
    {
        var db = CreatePostgresContainer();
        db.StartAsync().Wait();
        services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(db.GetConnectionString()));
        return db;
    }

    public static void Migrate(IServiceProvider serviceProvider)
    {
        serviceProvider
            .GetRequiredService<DatabaseContext>()
            .Database
            .Migrate();
    }

    public static PostgreSqlContainer CreatePostgresContainer()
    {
        return new PostgreSqlBuilder()
            .WithDatabase("postgres")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }
}