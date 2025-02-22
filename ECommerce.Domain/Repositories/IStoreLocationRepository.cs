using ECommerce.Domain.Entities.StoreLocation;
using ECommerce.Domain.Models.StoreLocation;

namespace ECommerce.Domain.Repositories.StoreLocation
{
    public interface IStoreLocationRepository
    {
        public Task<StoreLocationEntity?> CreateAsync(CreateStoreLocationModel storeLocation);
        public Task UpdateAsync(UpdateStoreLocationModel updateEntity);
        public Task DeleteAsync(int storeLocationId);
        public Task<List<StoreLocationEntity>> GetAll();
        public Task<StoreLocationEntity?> FindByIdAsync(int storeLocationId);
        public Task<StoreLocationEntity?> FindByNameAsync(string storeLocationName);
    }
}