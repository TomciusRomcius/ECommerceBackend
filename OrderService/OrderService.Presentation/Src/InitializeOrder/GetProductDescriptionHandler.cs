using MediatR;
using Microsoft.Extensions.Options;
using OrderService.Utils;

namespace OrderService.InitializeOrder;

public class GetProductDescriptionHandler : IRequestHandler<GetProductDescriptionQuery, Result<List<ProductPriceModel>>>
{
    private readonly ILogger<GetProductDescriptionHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly MicroserviceNetworkConfig _microserviceNetworkConfig;

    public GetProductDescriptionHandler(ILogger<GetProductDescriptionHandler> logger,
        HttpClient httpClient,
        IOptions<MicroserviceNetworkConfig> microserviceNetworkConfig)
    {
        _logger = logger;
        _httpClient = httpClient;
        _microserviceNetworkConfig = microserviceNetworkConfig.Value;
    }

    public async Task<Result<List<ProductPriceModel>>> Handle(
        GetProductDescriptionQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        HttpRequestMessage message = new HttpRequestMessage(
            HttpMethod.Get,
            $"{_microserviceNetworkConfig.ProductServiceUrl}/product"
        );
        var reqBody = new { ProductIds = request.ProductIds };
        message.Content = JsonContent.Create(reqBody);
        HttpResponseMessage res = await _httpClient.SendAsync(message, cancellationToken);
        if (!res.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Failed to get product description. Request: {@Request} Response: {@Response} ProductIds: {@ProductIds}",
                message,
                res,
                request.ProductIds
            );
            return new Result<List<ProductPriceModel>>([
                new ResultError(
                    ResultErrorType.UNKNOWN_ERROR,
                    "Unknown error."
                )
            ]);
        }

        string sRes = await res.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogDebug("Response: {}", sRes);
        List<ProductPriceModel>? productModels = JsonUtils.Deserialize<List<ProductPriceModel>>(sRes);
        return new Result<List<ProductPriceModel>>(productModels ?? new List<ProductPriceModel>());
    }
}