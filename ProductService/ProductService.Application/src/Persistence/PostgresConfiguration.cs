namespace ProductService.Application.Persistence;

public class PostgresConfiguration
{
    public required string Host { get; set; }
    public required string Database { get; set; }
    public int Port { get; set; } = 5432;
    public required string Username { get; set; }
    public required string Password { get; set; }
}