using ECommerce.DataAccess.Models.StoreLocation;

namespace ECommerce.StoreLocation
{
    public interface IStoreLocationService
    {
        public Task<StoreLocationModel?> CreateStoreLocation(CreateStoreLocationModel storeLocation);
        public Task RemoveLocation(int storeLocationId);
        public Task UpdateStoreLocation(UpdateStoreLocationModel updateStoreLocation);
        public Task<List<StoreLocationModel>> GetLocations();
    }
}
