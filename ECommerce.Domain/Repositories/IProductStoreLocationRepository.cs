using ECommerce.Domain.Entities.CartProduct;
using ECommerce.Domain.Entities.ProductStoreLocation;
using ECommerce.Domain.Models.ProductStoreLocation;

namespace ECommerce.Domain.Repositories.ProductStoreLocation
{
    public interface IProductStoreLocationRepository
    {
        public Task AddProductToStore(ProductStoreLocationEntity Entity);
        public Task RemoveProductFromStore(int storeLocationId, int productId);
        public Task UpdateProduct(ProductStoreLocationEntity Entity);
        public Task UpdateStock(List<CartProductEntity> cartProducts);
        public Task<List<DetailedProductModel>> GetProductsFromStoreAsync(int storeLocationId);
        public Task<List<ProductStoreLocationEntity>> GetProductsFromStoreAsync(List<(int, int)> storeLocationIdProductId);
        public Task<List<int>> GetProductIdsFromStoreAsync(int storeLocationId);
    }
}