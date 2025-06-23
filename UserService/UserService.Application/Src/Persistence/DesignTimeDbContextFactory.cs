using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using UserService.Application.Utils;

namespace UserService.Application.Persistence;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        string? host = args.FirstOrDefault(arg => arg.StartsWith("--host"))?.Split('=')[1];
        string? database = args.FirstOrDefault(arg => arg.StartsWith("--database"))?.Split('=')[1];
        string? username = args.FirstOrDefault(arg => arg.StartsWith("--username"))?.Split('=')[1];
        string? password = args.FirstOrDefault(arg => arg.StartsWith("--password"))?.Split('=')[1];
        
        string? environment = args.FirstOrDefault(arg => arg.StartsWith("environment"))?.Split('=')[1];
        if (environment == "Production")
        {
            ArgumentNullException.ThrowIfNull(host);
            ArgumentNullException.ThrowIfNull(database);
            ArgumentNullException.ThrowIfNull(username);
            ArgumentNullException.ThrowIfNull(password);
        }
        
        PostgresConfiguration configuration = new PostgresConfiguration
        {
            Host = host ?? "postgres",
            Database = database ?? "postgres",
            Username = username ?? "postgres",
            Password = password ?? "postgres",
        };
        
        return new DatabaseContext(Options.Create(configuration));
    }
}