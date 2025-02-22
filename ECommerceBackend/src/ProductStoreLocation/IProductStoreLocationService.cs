using ECommerce.Domain.Entities.ProductStoreLocation;
using ECommerce.Domain.Models.ProductStoreLocation;

namespace ECommerce.ProductStoreLocation
{
    public interface IProductStoreLocationService
    {
        public Task AddProductToStore(ProductStoreLocationEntity model);
        public Task RemoveProductFromStore(int storeLocationId, int productId);
        public Task ModifyProductFromStore(ProductStoreLocationEntity model);
        public Task<List<DetailedProductModel>> GetProductsFromStore(int storeLocationId);
        public Task<List<int>> GetProductIdsFromStore(int storeLocationId);

    }
}