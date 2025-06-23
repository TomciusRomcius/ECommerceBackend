using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ECommerce.Persistence.src
{
    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            string? envPath = GetClosestEnvFile();
            if (envPath != null)
            {
                DotEnv.Load(
                    new DotEnvOptions(
                        envFilePaths: new[] { Path.Combine(new[] { envPath }) },
                        overwriteExistingVars: false
                    )
                );
            }

            string? host = Environment.GetEnvironmentVariable("Database__Host");
            string? database = Environment.GetEnvironmentVariable("Database__Database");
            string? username = Environment.GetEnvironmentVariable("Database__Username");
            string? password = Environment.GetEnvironmentVariable("Database__Password");

            string connectionString = $"Host={host};Database={database};Username={username};Password={password}";

            return new DatabaseContext(
                new DbContextOptionsBuilder<DatabaseContext>().UseNpgsql(connectionString).Options
            );
        }

        private static string? GetClosestEnvFile()
        {
            const int maxDepth = 5;
            var currentDir = new DirectoryInfo(Environment.CurrentDirectory);

            int depth = 0;
            while (currentDir != null && depth < maxDepth)
            {
                var envPath = Path.Combine(currentDir.FullName, ".env");
                if (File.Exists(envPath))
                {
                    return envPath;
                }

                currentDir = currentDir.Parent;
                depth++;
            }

            return null; // Not found
        }
    }
}
