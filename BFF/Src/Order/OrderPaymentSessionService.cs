using System.Net.Http.Headers;
using ECommerceBackend.Utils.Microservices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BFF.Order;

public class OrderPaymentSessionService(HttpClient httpClient, IOptions<MicroserviceHosts> hosts)
    : IOrderPaymentSessionService
{
    public async Task<HttpResponseMessage> CreateOrderPaymentSessionAsync(
        bool testCharge,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var query = new QueryString().Add("testcharge", testCharge ? "true" : "false");
        string url = $"{hosts.Value.OrderServiceUrl}/Order/session{query}";

        using var request = new HttpRequestMessage(HttpMethod.Post, url);

        if (!string.IsNullOrWhiteSpace(authorizationHeader))
        {
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(authorizationHeader);
        }

        return await httpClient.SendAsync(request, cancellationToken);
    }
}
