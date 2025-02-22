
using ECommerce.Domain.Entities.ShippingAddress;
using ECommerce.Domain.Models.ShippingAddress;

namespace ECommerce.Domain.Repositories.ShippingAddress
{
    public interface IShippingAddressRepository
    {
        public Task AddAddressAsync(ShippingAddressEntity addressEntity);
        public Task UpdateAddressAsync(UpdateShippingAddressModel updateAddressEntity);
        public Task<List<ShippingAddressEntity>> GetAddresses(string userId);
        public Task DeleteAddressAsync(string userId, bool isShipping);
    }
}