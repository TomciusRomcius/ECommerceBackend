namespace ECommerce.Infrastructure.Services;

public class PostgresConfiguration
{
    public PostgresConfiguration(string host, string username, string password, string database)
    {
        Host = host;
        Username = username;
        Password = password;
        Database = database;
    }

    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }
    public int Port { get; set; } = 5432;
}