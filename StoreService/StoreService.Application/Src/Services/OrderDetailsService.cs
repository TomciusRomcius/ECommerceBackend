using ECommerceBackend.Utils.Database;
using Newtonsoft.Json;

namespace StoreService.Application.Services;

public class OrderDetailsService : IOrderDetailsService
{
    private readonly HttpClient _httpClient;
    private readonly MicroserviceHosts _serviceHosts;

    public OrderDetailsService(HttpClient httpClient, MicroserviceHosts serviceHosts)
    {
        _httpClient = httpClient;
        _serviceHosts = serviceHosts;
    }

    public async Task<GetOrdersResponseType> FetchAsync(string userId, string orderId, CancellationToken cancellationToken)
    {
        HttpResponseMessage request = await _httpClient.GetAsync(
            $"{_serviceHosts.OrderServiceUrl}/order?userId={userId}&orderId={orderId}",
            cancellationToken);
        string reqString = await request.Content.ReadAsStringAsync(cancellationToken);
        GetOrdersResponseType? responseDeserialized = JsonConvert.DeserializeObject<GetOrdersResponseType>(reqString);
        return responseDeserialized ?? throw new InvalidDataException($"Failed to deserialize request: {reqString}");
    }
}
