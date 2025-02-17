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
    public class ShippingAddressRepositoryTest
    {
        [Fact]
        public async Task ShouldSuccesfullyCreateAndRetrieveTheShippingAddress()
        {
            var testContainer = new TestDatabase();

            // Create base user to whom we are going to attach a shipping address
            var userRepository = new UserRepository(
                testContainer._postgresService,
                new Mock<ILogger>().Object
            );

            var userId = new Guid();

            await userRepository.CreateAsync(
                new UserModel(userId.ToString(), "email@gmail.com", "passwordhash", "firstname", "lastname")
            );

            // Create a shipping address
            var shippingAddressRepository = new ShippingAddressRepository(testContainer._postgresService);

            var shippingAddress = new ShippingAddressModel
            {
                UserId = userId.ToString(),
                RecipientName = "RecipientName",
                StreetAddress = "StreetAddress",
                ApartmentUnit = "ApartmentUnit",
                City = "City",
                State = "State",
                PostalCode = "Postal",
                Country = "Country",
                MobileNumber = "Mobile",
            };

            await shippingAddressRepository.AddAddressAsync(shippingAddress);

            var addresses = await shippingAddressRepository.GetAddresses(userId.ToString());
            var retrieved = addresses.Where(item => item.ShippingAddressId == shippingAddress.ShippingAddressId).ToList()[0];

            Assert.Equal(shippingAddress.UserId, retrieved.UserId);
            Assert.Equal(shippingAddress.RecipientName, retrieved.RecipientName);
            Assert.Equal(shippingAddress.StreetAddress, retrieved.StreetAddress);
            Assert.Equal(shippingAddress.ApartmentUnit, retrieved.ApartmentUnit);
            Assert.Equal(shippingAddress.City, retrieved.City);
            Assert.Equal(shippingAddress.State, retrieved.State);
            Assert.Equal(shippingAddress.PostalCode, retrieved.PostalCode);
            Assert.Equal(shippingAddress.Country, retrieved.Country);
            Assert.Equal(shippingAddress.MobileNumber, retrieved.MobileNumber);
        }
    }
}