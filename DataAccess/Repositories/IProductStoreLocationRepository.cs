using ECommerce.DataAccess.Entities.CartProduct;
using ECommerce.DataAccess.Models.ProductStoreLocation;

namespace ECommerce.DataAccess.Repositories.ProductStoreLocation
{
    public interface IProductStoreLocationRepository
    {
        public Task AddProductToStore(ProductStoreLocationModel model);
        public Task RemoveProductFromStore(int storeLocationId, int productId);
        public Task UpdateProduct(ProductStoreLocationModel model);
        public Task<int> UpdateStock(List<CartProductEntity> cartProducts);
        public Task<List<DetailedProductModel>> GetProductsFromStoreAsync(int storeLocationId);
        public Task<List<ProductStoreLocationModel>> GetProductsFromStoreAsync(List<(int, int)> storeLocationIdProductId);
        public Task<List<int>> GetProductIdsFromStoreAsync(int storeLocationId);
    }
}