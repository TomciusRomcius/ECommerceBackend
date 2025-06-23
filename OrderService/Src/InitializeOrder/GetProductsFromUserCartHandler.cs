using System.Net.Http.Headers;
using MediatR;
using Microsoft.Extensions.Options;
using OrderService.Utils;

namespace OrderService.InitializeOrder;

public class GetProductsFromUserCartHandler : IRequestHandler<GetProductsFromUserCartQuery, Result<List<CartProductMinimalModel>>>
{
    private readonly ILogger<GetProductsFromUserCartHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly MicroserviceNetworkConfig _microserviceNetworkConfig;
    
    public GetProductsFromUserCartHandler(ILogger<GetProductsFromUserCartHandler> logger,
        HttpClient httpClient,
        IOptions<MicroserviceNetworkConfig> microserviceNetworkConfig)
    {
        _logger = logger;
        _httpClient = httpClient;
        _microserviceNetworkConfig = microserviceNetworkConfig.Value;
    }
    
    public async Task<Result<List<CartProductMinimalModel>>> Handle(
        GetProductsFromUserCartQuery request,
        CancellationToken cancellationToken)
    {
        // TODO: more descriptive errors on fail
        _logger.LogTrace("Entered Handle");
        _logger.LogDebug("Getting cart items from user: {UserId}", request.JwtToken);
        var message = new HttpRequestMessage(HttpMethod.Get, $"{_microserviceNetworkConfig.UserServiceUrl}/cart");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", request.JwtToken);
        HttpResponseMessage response = await _httpClient.SendAsync(message, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get user cart items.");
            _logger.LogDebug("Failed to get user cart items. Response: {@Response}", response);
            return new Result<List<CartProductMinimalModel>>([
                new ResultError(ResultErrorType.UNKNOWN_ERROR, "Failed to get user cart items")
            ]);
        }
        string sResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        List<CartProductMinimalModel>? cartProducts = JsonUtils.Deserialize<List<CartProductMinimalModel>>(sResponse);
        return new Result<List<CartProductMinimalModel>>(cartProducts ?? []);
    }
}