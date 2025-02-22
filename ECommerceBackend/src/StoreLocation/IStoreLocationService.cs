using ECommerce.Domain.Entities.StoreLocation;
using ECommerce.Domain.Models.StoreLocation;

namespace ECommerce.StoreLocation
{
    public interface IStoreLocationService
    {
        public Task<StoreLocationEntity?> CreateStoreLocation(CreateStoreLocationModel storeLocation);
        public Task RemoveLocation(int storeLocationId);
        public Task UpdateStoreLocation(UpdateStoreLocationModel updateStoreLocation);
        public Task<List<StoreLocationEntity>> GetLocations();
    }
}
