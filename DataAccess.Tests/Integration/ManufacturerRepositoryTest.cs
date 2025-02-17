using DataAccess.Test.Integration.Utils;
using ECommerce.DataAccess.Models.Manufacturer;
using ECommerce.DataAccess.Repositories;

namespace DataAccess.Tests.Integration
{
    public class ManufacturerRepositoryTest
    {
        [Fact]
        public async Task ShouldSuccesfullyCreateAndRetrieveManufacturer()
        {
            var testContainer = new TestDatabase();

            ManufacturerRepository manufacturerRepository = new ManufacturerRepository(testContainer._postgresService);
            string name = "manufacturer name";

            // Create manufacturer
            ManufacturerModel? manufacturer = await manufacturerRepository.CreateAsync(name);

            Assert.NotNull(manufacturer);
            Assert.Equal(name, manufacturer.Name);

            // Find manufacturer
            ManufacturerModel? retrieved = await manufacturerRepository.FindByNameAsync(name);

            Assert.NotNull(retrieved);
            Assert.Equal(manufacturer.Name, retrieved.Name);
            Assert.Equal(manufacturer.ManufacturerId, retrieved.ManufacturerId);

            await testContainer.DisposeAsync();
        }
    }
}