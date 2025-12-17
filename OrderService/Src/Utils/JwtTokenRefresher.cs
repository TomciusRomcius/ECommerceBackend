using System.Text.Json;
using Microsoft.Extensions.Options;

namespace OrderService.Utils;

public class JwtTokenRefresher : IHostedService
{
    private readonly ILogger<JwtTokenRefresher> _logger;
    private readonly JwtConfig _jwtConfig;
    private readonly HttpClient _httpClient;
    private readonly InternalJwtTokenContainer _jwtTokenContainer;

    public JwtTokenRefresher(ILogger<JwtTokenRefresher> logger,
        IOptions<JwtConfig> jwtConfig,
        HttpClient httpClient,
        InternalJwtTokenContainer jwtTokenContainer)
    {
        _logger = logger;
        _jwtConfig = jwtConfig.Value;
        _httpClient = httpClient;
        _jwtTokenContainer = jwtTokenContainer;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered: {}", nameof(StartAsync));
        await ReceiveToken(cancellationToken);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_jwtTokenContainer.ExpirationDate < DateTime.Now)
            {
                await ReceiveToken(cancellationToken);
            }
            else
            {
                TimeSpan expiresIn = _jwtTokenContainer.ExpirationDate - DateTime.Now;
                _logger.LogDebug("Waiting for next token fetch for {} seconds", expiresIn.TotalSeconds);
                await Task.Delay(expiresIn, cancellationToken);
            }
            // Get access token
        }
    }

    private async Task ReceiveToken(CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered: {}", nameof(ReceiveToken));
        string url = $"{Path.Combine(_jwtConfig.Authority, "protocol/openid-connect/token")}";
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
        {
            new("client_id", _jwtConfig.ClientId),
            new("client_secret", _jwtConfig.SecretClientId),
            new("grant_type", "client_credentials"),
            new("audience", _jwtConfig.Audience),
        });
        HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        JsonElement contentJson = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        string? accessToken = contentJson.GetProperty("access_token").GetString();
        int expiresIn = contentJson.GetProperty("expires_in").GetInt32();
        DateTime expiresAt = DateTime.Now.AddSeconds(expiresIn - 5);
        _jwtTokenContainer.ExpirationDate = expiresAt;
        _logger.LogDebug("Received access token for {} seconds", expiresIn);
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}