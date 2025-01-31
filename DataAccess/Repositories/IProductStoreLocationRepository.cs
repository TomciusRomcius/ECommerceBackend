using ECommerce.DataAccess.Models.ProductStoreLocation;

namespace ECommerce.DataAccess.Repositories.ProductStoreLocation
{
    public interface IProductStoreLocationRepository
    {
        public Task AddProductToStore(ProductStoreLocationModel model);
        public Task RemoveProductFromStore(string storeLocationId, string productId);
        public Task UpdateProduct(ProductStoreLocationModel model);
    }
}