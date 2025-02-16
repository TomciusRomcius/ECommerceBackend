using System.Data;
using DataAccess.Test.Integration.Utils;
using ECommerce.DataAccess.Models.ShippingAddress;
using ECommerce.DataAccess.Models.User;
using ECommerce.DataAccess.Repositories;
using ECommerce.DataAccess.Repositories.ShippingAddress;
using Microsoft.Extensions.Logging;
using Moq;

namespace DataAccess.Test.Integration.ShippingAddressRepositoryTest
{
    [Collection("Shipping address creation flow")]
    public class ShippingAddressRepositoryTest : IAsyncLifetime
    {
        TestContainerPostgresServiceWrapper? _testContainer;
        ShippingAddressRepository? _shippingAddressRepository;
        Guid _userId = new Guid();
        ShippingAddressModel? _address = null;

        public async Task InitializeAsync()
        {
            _testContainer = await TestContainerPostgresServiceWrapper.CreateAsync();
            _shippingAddressRepository = new ShippingAddressRepository(_testContainer._postgresService);

            var userRepository = new UserRepository(
                _testContainer._postgresService,
                new Mock<ILogger>().Object
            );

            // Create base user to whom we are going to attach a shipping address
            await userRepository.CreateAsync(
                new UserModel(_userId.ToString(), "email@gmail.com", "passwordhash", "firstname", "lastname")
            );
        }

        public async Task DisposeAsync()
        {
            await _testContainer.DisposeAsync();
        }

        [Fact]
        public async Task ShouldSuccesfullyCreateAndRetrieveTheShippingAddress()
        {

            _address = new ShippingAddressModel
            {
                UserId = _userId.ToString(),
                RecipientName = "RecipientName",
                StreetAddress = "StreetAddress",
                ApartmentUnit = "ApartmentUnit",
                City = "City",
                State = "State",
                PostalCode = "Postal",
                Country = "Country",
                MobileNumber = "Mobile",
            };

            await _shippingAddressRepository.AddAddressAsync(_address);

            if (_address is null)
            {
                throw new DataException("Shipping address is null! Cannot proceed with the test");
            }

            var addresses = await _shippingAddressRepository.GetAddresses(_userId.ToString());
            var result = addresses.Where(item => item.ShippingAddressId == _address.ShippingAddressId).ToList()[0];

            Assert.Equal(_address.UserId, result.UserId);
            Assert.Equal(_address.RecipientName, result.RecipientName);
            Assert.Equal(_address.StreetAddress, result.StreetAddress);
            Assert.Equal(_address.ApartmentUnit, result.ApartmentUnit);
            Assert.Equal(_address.City, result.City);
            Assert.Equal(_address.State, result.State);
            Assert.Equal(_address.PostalCode, result.PostalCode);
            Assert.Equal(_address.Country, result.Country);
            Assert.Equal(_address.MobileNumber, result.MobileNumber);
        }
    }
}