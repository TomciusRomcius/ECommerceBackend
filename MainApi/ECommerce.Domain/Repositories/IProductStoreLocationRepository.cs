using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Utils;

namespace ECommerce.Domain.Repositories;

public interface IProductStoreLocationRepository
{
    public Task<ResultError?> AddProductToStore(ProductStoreLocationEntity entity);
    public Task RemoveProductFromStore(int storeLocationId, int productId);
    public Task UpdateProduct(ProductStoreLocationEntity entity);
    public Task UpdateStock(List<CartProductEntity> cartProducts);
    public Task<List<DetailedProductModel>> GetProductsFromStoreAsync(int storeLocationId);
    public Task<List<ProductStoreLocationEntity>> GetProductsFromStoreAsync(List<(int, int)> storeLocationIdProductId);
    public Task<List<int>> GetProductIdsFromStoreAsync(int storeLocationId);
}