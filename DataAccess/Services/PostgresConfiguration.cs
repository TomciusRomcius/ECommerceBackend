public class PostgresConfiguration
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }

    public PostgresConfiguration(string host, string username, string password, string database)
    {
        Host = host;
        Username = username;
        Password = password;
        Database = database;
    }
}