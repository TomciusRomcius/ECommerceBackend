using System.Data;
using DataAccess.Test.Integration.Utils;
using ECommerce.DataAccess.Models.ShippingAddress;
using ECommerce.DataAccess.Models.User;
using ECommerce.DataAccess.Repositories;
using ECommerce.DataAccess.Repositories.ShippingAddress;
using Microsoft.Extensions.Logging;
using Moq;

namespace DataAccess.Tests.Integration
{
    public class DataFixture : IAsyncLifetime
    {
        public TestDatabase _testContainer { get; }
        public ShippingAddressRepository? _shippingAddressRepository { get; }
        public Guid _userId { get; set; } = new Guid();
        public ShippingAddressModel? _address { get; set; } = null;

        public DataFixture()
        {
            _testContainer = new TestDatabase();
            _shippingAddressRepository = new ShippingAddressRepository(_testContainer._postgresService);
        }

        public async Task InitializeAsync()
        {
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
    }

    [Collection("Shipping address creation flow")]
    public class ShippingAddressRepositoryTest : IClassFixture<DataFixture>
    {
        readonly DataFixture _dataFixture;
        public ShippingAddressRepositoryTest(DataFixture dataFixture)
        {
            _dataFixture = dataFixture;
        }

        [Fact]
        public async Task ShouldSuccesfullyCreateAndRetrieveTheShippingAddress()
        {

            _dataFixture._address = new ShippingAddressModel
            {
                UserId = _dataFixture._userId.ToString(),
                RecipientName = "RecipientName",
                StreetAddress = "StreetAddress",
                ApartmentUnit = "ApartmentUnit",
                City = "City",
                State = "State",
                PostalCode = "Postal",
                Country = "Country",
                MobileNumber = "Mobile",
            };

            await _dataFixture._shippingAddressRepository.AddAddressAsync(_dataFixture._address);

            if (_dataFixture._address is null)
            {
                throw new DataException("Shipping address is null! Cannot proceed with the test");
            }

            var addresses = await _dataFixture._shippingAddressRepository.GetAddresses(_dataFixture._userId.ToString());
            var result = addresses.Where(item => item.ShippingAddressId == _dataFixture._address.ShippingAddressId).ToList()[0];

            Assert.Equal(_dataFixture._address.UserId, result.UserId);
            Assert.Equal(_dataFixture._address.RecipientName, result.RecipientName);
            Assert.Equal(_dataFixture._address.StreetAddress, result.StreetAddress);
            Assert.Equal(_dataFixture._address.ApartmentUnit, result.ApartmentUnit);
            Assert.Equal(_dataFixture._address.City, result.City);
            Assert.Equal(_dataFixture._address.State, result.State);
            Assert.Equal(_dataFixture._address.PostalCode, result.PostalCode);
            Assert.Equal(_dataFixture._address.Country, result.Country);
            Assert.Equal(_dataFixture._address.MobileNumber, result.MobileNumber);
        }
    }
}