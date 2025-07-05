using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Models;
using ECommerce.Domain.src.Utils;

namespace ECommerce.Domain.src.Repositories;

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