using ECommerce.Application.src.Utils;
using ECommerce.Domain.src.Repositories;
using ECommerce.Infrastructure.src.Repositories;
using ECommerce.Infrastructure.src.Services;
using ECommerce.Persistence.src;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Presentation.src.Initialization;

public static class DataAccessInitialization
{
    public static void InitDb(WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<PostgresConfiguration>()
            .Bind(builder.Configuration.GetSection("Database"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        PostgresConfiguration? cfg = builder.Configuration.GetSection("Database").Get<PostgresConfiguration>();
        if (cfg == null)
        {
            throw new InvalidDataException("Postgres configuration missing");
        }
        builder.Services.AddTransient<IPostgresService, PostgresService>();

        string conString = @$"
            Host={cfg.Host};
            Database={cfg.Database};
            Username={cfg.Username};
            Password={cfg.Password};
        ";

        builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(conString));
    }

    public static void InitRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IProductStoreLocationRepository, ProductStoreLocationRepository>();
    }
}