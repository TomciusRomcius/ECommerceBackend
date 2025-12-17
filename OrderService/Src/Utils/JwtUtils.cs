using System.ComponentModel.DataAnnotations;

namespace OrderService.Utils;

public class JwtConfig
{
    [Required]
    public required string ClientId { get; init; }
    [Required]
    public required string SecretClientId { get; init; }
    [Required]
    public required string Audience { get; init; }
    [Required]
    public required string Issuer { get; init; }
    [Required]
    public required string Authority { get; init; }
}

public class InternalJwtTokenContainer()
{
    public string AccessToken { get; set; } = "";
    public DateTime ExpirationDate { get; set; }
}

public class JwtTokenContainerReader
{
    private readonly ILogger<JwtTokenContainerReader> _logger;
    private readonly InternalJwtTokenContainer _jwtTokenContainer;

    public JwtTokenContainerReader(ILogger<JwtTokenContainerReader> logger, InternalJwtTokenContainer jwtTokenContainer)
    {
        _logger = logger;
        _jwtTokenContainer = jwtTokenContainer;
    }

    public string AccessToken => _jwtTokenContainer.AccessToken;
}