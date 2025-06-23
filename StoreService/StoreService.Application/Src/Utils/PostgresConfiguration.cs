using System.ComponentModel.DataAnnotations;

namespace StoreService.Application.Utils;

public class PostgresConfiguration
{
    [Required] public required string Host { get; set; } = "postgres";

    [Required] public required string Database { get; set; } = "postgres";

    public int Port { get; set; } = 5432;

    [Required] public required string Username { get; set; } = "postgres";

    [Required] public required string Password { get; set; } = "postgres";
}