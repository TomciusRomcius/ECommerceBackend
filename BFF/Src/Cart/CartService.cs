using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ECommerceBackend.Utils.Microservices;
using Microsoft.Extensions.Options;

namespace BFF.Cart;

public class CartService(HttpClient httpClient, IOptions<MicroserviceHosts> hosts, ILogger<CartService> logger)
    : ICartService
{
    public async Task<IReadOnlyList<CartItemWithProductDto>> GetItemsWithProductsAsync(
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        string cartUrl = $"{hosts.Value.UserServiceUrl}/cart";
        using HttpRequestMessage cartRequest = CreateAuthorizedRequest(HttpMethod.Get, cartUrl, authorizationHeader);
        using HttpResponseMessage cartResponse = await httpClient.SendAsync(cartRequest, cancellationToken);
        cartResponse.EnsureSuccessStatusCode();

        List<CartProductDto>? cartItems =
            await cartResponse.Content.ReadFromJsonAsync<List<CartProductDto>>(cancellationToken);

        if (cartItems is null || cartItems.Count == 0)
        {
            return [];
        }

        var query = new QueryString();
        foreach (int productId in cartItems.Select(item => item.ProductId).Distinct())
        {
            query = query.Add("ids", productId.ToString());
        }

        string productsUrl = $"{hosts.Value.ProductServiceUrl}/product/by-ids{query}";
        using HttpResponseMessage productsResponse = await httpClient.GetAsync(productsUrl, cancellationToken);
        productsResponse.EnsureSuccessStatusCode();

        List<JsonElement>? products =
            await productsResponse.Content.ReadFromJsonAsync<List<JsonElement>>(cancellationToken);

        Dictionary<int, JsonElement> productsById = (products ?? [])
            .Where(product => product.TryGetProperty("productId", out _))
            .ToDictionary(product => product.GetProperty("productId").GetInt32());

        return cartItems
            .Select(item => new CartItemWithProductDto
            {
                UserId = item.UserId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Product = productsById.TryGetValue(item.ProductId, out JsonElement product) ? product : null,
            })
            .ToList();
    }

    public async Task<HttpResponseMessage> AddItemAsync(
        AddCartItemRequest request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        int storeLocationId = await ResolveStoreLocationIdAsync(request.ProductId, cancellationToken);

        string cartUrl = $"{hosts.Value.UserServiceUrl}/cart";
        using HttpRequestMessage cartRequest = CreateAuthorizedRequest(HttpMethod.Post, cartUrl, authorizationHeader);
        cartRequest.Content = JsonContent.Create(new
        {
            request.ProductId,
            storeLocationId,
            request.Quantity,
        });

        return await httpClient.SendAsync(cartRequest, cancellationToken);
    }

    public async Task<HttpResponseMessage> RemoveItemAsync(
        int productId,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        int storeLocationId = await ResolveStoreLocationIdAsync(productId, cancellationToken);

        var query = new QueryString()
            .Add("productId", productId.ToString())
            .Add("storeLocationId", storeLocationId.ToString());

        string cartUrl = $"{hosts.Value.UserServiceUrl}/cart{query}";
        using HttpRequestMessage cartRequest = CreateAuthorizedRequest(HttpMethod.Delete, cartUrl, authorizationHeader);
        return await httpClient.SendAsync(cartRequest, cancellationToken);
    }

    private async Task<int> ResolveStoreLocationIdAsync(int productId, CancellationToken cancellationToken)
    {
        var query = new QueryString().Add("ids", productId.ToString());
        string productUrl = $"{hosts.Value.ProductServiceUrl}/product/by-ids{query}";

        using HttpResponseMessage productResponse = await httpClient.GetAsync(productUrl, cancellationToken);
        productResponse.EnsureSuccessStatusCode();

        List<JsonElement>? products =
            await productResponse.Content.ReadFromJsonAsync<List<JsonElement>>(cancellationToken);

        JsonElement product = products?.FirstOrDefault()
            ?? throw new InvalidOperationException($"Product {productId} was not found.");

        if (!product.TryGetProperty("store", out JsonElement store) ||
            !store.TryGetProperty("storeLocationId", out JsonElement storeLocationIdElement))
        {
            throw new InvalidOperationException($"Product {productId} does not have store details.");
        }

        int storeLocationId = storeLocationIdElement.GetInt32();
        logger.LogDebug("Resolved store location {StoreLocationId} for product {ProductId}", storeLocationId, productId);
        return storeLocationId;
    }

    private static HttpRequestMessage CreateAuthorizedRequest(
        HttpMethod method,
        string url,
        string? authorizationHeader)
    {
        var request = new HttpRequestMessage(method, url);

        if (!string.IsNullOrWhiteSpace(authorizationHeader))
        {
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(authorizationHeader);
        }

        return request;
    }
}
