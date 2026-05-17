using ECommerceBackend.Utils.Pagination;

namespace BFF.StoreProducts;

public interface IStoreProductsService
{
    Task<Page<StoreProductDto>> GetProductsAsync(
        int? storeLocationId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<StoreProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default);
}
