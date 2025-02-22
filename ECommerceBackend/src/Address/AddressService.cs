using ECommerce.Domain.Entities.ShippingAddress;
using ECommerce.Domain.Models.ShippingAddress;
using ECommerce.Domain.Repositories.ShippingAddress;

namespace ECommerce.Address
{
    public interface IAddressService
    {
        /// <returns>A billing and shipping address</returns>
        public Task<List<ShippingAddressEntity>> GetAddresses(string userId);
        public Task AddAddress(ShippingAddressEntity address);
        public Task UpdateAddress(UpdateShippingAddressModel updateAddressModel);
        public Task DeleteAddress(string userId, bool isShipping);
    }

    public class AddressService : IAddressService
    {
        readonly IShippingAddressRepository _addressRepository;

        public AddressService(IShippingAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task AddAddress(ShippingAddressEntity address)
        {
            await _addressRepository.AddAddressAsync(address);
        }

        public async Task DeleteAddress(string userId, bool isShipping)
        {
            await _addressRepository.DeleteAddressAsync(userId, isShipping);
        }

        public async Task<List<ShippingAddressEntity>> GetAddresses(string userId)
        {
            return await _addressRepository.GetAddresses(userId);
        }

        public async Task UpdateAddress(UpdateShippingAddressModel updateAddressModel)
        {
            await _addressRepository.UpdateAddressAsync(updateAddressModel);
        }
    }
}