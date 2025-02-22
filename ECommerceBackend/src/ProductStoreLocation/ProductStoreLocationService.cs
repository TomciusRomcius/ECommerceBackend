using ECommerce.Domain.Entities.ProductStoreLocation;
using ECommerce.Domain.Models.ProductStoreLocation;
using ECommerce.Domain.Repositories.ProductStoreLocation;

namespace ECommerce.ProductStoreLocation
{
    public class ProductStoreLocationService : IProductStoreLocationService
    {
        readonly IProductStoreLocationRepository _productStoreLocationRepository;

        public ProductStoreLocationService(IProductStoreLocationRepository productStoreLocationRepository)
        {
            _productStoreLocationRepository = productStoreLocationRepository;
        }

        public async Task AddProductToStore(ProductStoreLocationEntity model)
        {
            await _productStoreLocationRepository.AddProductToStore(model);
        }

        public async Task<List<DetailedProductModel>> GetProductsFromStore(int storeLocationId)
        {
            return await _productStoreLocationRepository.GetProductsFromStoreAsync(storeLocationId);
        }

        public async Task<List<int>> GetProductIdsFromStore(int storeLocationId)
        {
            return await _productStoreLocationRepository.GetProductIdsFromStoreAsync(storeLocationId);
        }

        public async Task ModifyProductFromStore(ProductStoreLocationEntity model)
        {
            await _productStoreLocationRepository.UpdateProduct(model);
        }

        public async Task RemoveProductFromStore(int storeLocationId, int productId)
        {
            await _productStoreLocationRepository.RemoveProductFromStore(storeLocationId, productId);
        }
    }
}