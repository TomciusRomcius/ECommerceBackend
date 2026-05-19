namespace BFF.Cart;

public interface ICartService
{
    Task<IReadOnlyList<CartItemWithProductDto>> GetItemsWithProductsAsync(
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> AddItemAsync(
        AddCartItemRequest request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> RemoveItemAsync(
        int productId,
        int storeLocationId,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);
}
