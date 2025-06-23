using ECommerce.Application.src.Utils;
using ECommerce.Infrastructure.src.Services;
using ECommerce.Persistence;
using ECommerce.Persistence.src;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Presentation.Initialization;

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

    public static async Task WaitForConnectionAndAppliedMigrations(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            int maxTries = 12;
            int timeoutSeconds = 10;
            var ctx = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            for (int i = 0; i < maxTries; i++)
            {
                if (ctx.Database.CanConnect() && !ctx.Database.GetPendingMigrations().Any())
                {
                    break;
                }
                await Task.Delay(TimeSpan.FromSeconds(timeoutSeconds));
            }
        }
    }
}