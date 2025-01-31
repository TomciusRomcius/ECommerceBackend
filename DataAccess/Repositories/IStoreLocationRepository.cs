using ECommerce.DataAccess.Models.StoreLocation;

namespace ECommerce.DataAccess.Repositories.StoreLocation
{
    public interface IStoreLocationRepository
    {
        public Task<StoreLocationModel?> CreateAsync(CreateStoreLocationModel storeLocation);
        public Task UpdateAsync(UpdateStoreLocationModel updateModel);
        public Task DeleteAsync(int storeLocationId);
        public Task<List<StoreLocationModel>> GetAll();
        public Task<StoreLocationModel?> FindByIdAsync(int storeLocationId);
        public Task<StoreLocationModel?> FindByNameAsync(string storeLocationName);
    }
}