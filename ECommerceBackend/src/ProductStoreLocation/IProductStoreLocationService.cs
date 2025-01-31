using ECommerce.DataAccess.Models.ProductStoreLocation;

namespace ECommerce.ProductStoreLocation
{
    public interface IProductStoreLocationService
    {
        public Task AddProductToStore(ProductStoreLocationModel model);
        public Task RemoveProductFromStore(int storeLocationId, int productId);
        public Task ModifyProductFromStore(ProductStoreLocationModel model);
        public Task<List<DetailedProductModel>> GetProductsFromStore(int storeLocationId);
        public Task<List<int>> GetProductIdsFromStore(int storeLocationId);

    }
}