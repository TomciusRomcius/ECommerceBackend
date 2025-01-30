using ECommerce.DataAccess.Models.Address;
using ECommerce.DataAccess.Repositories;

namespace ECommerce.Address
{
    public interface IAddressService
    {
        /// <returns>A billing and shipping address</returns>
        public Task<List<AddressModel>> GetAddresses(string userId);
        public Task AddAddress(AddressModel address);
        public Task UpdateAddress(UpdateAddressModel updateAddressModel);
        public Task DeleteAddress(string userId, bool isShipping);
    }

    public class AddressService : IAddressService
    {
        readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task AddAddress(AddressModel address)
        {
            await _addressRepository.AddAddressAsync(address);
        }

        public async Task DeleteAddress(string userId, bool isShipping)
        {
            await _addressRepository.DeleteAddressAsync(userId, isShipping);
        }

        public async Task<List<AddressModel>> GetAddresses(string userId)
        {
            return await _addressRepository.GetAddresses(userId);
        }

        public async Task UpdateAddress(UpdateAddressModel updateAddressModel)
        {
            await _addressRepository.UpdateAddressAsync(updateAddressModel);
        }
    }
}