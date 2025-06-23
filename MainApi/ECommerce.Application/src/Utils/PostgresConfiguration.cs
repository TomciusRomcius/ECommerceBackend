using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.src.Utils;

public class PostgresConfiguration
{
    public PostgresConfiguration() { }

    [Required]
    public required string Host { get; init; }
    [Required]
    public required string Username { get; init; }
    [Required]
    public required string Password { get; init; }
    [Required]
    public required string Database { get; init; }
    [Required]
    public required int Port { get; init; }
}