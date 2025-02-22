using ECommerce.TestUtils.TestDatabase;
using ECommerce.DataAccess.Repositories;
using ECommerce.Domain.Entities.Manufacturer;

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
            ManufacturerEntity? manufacturer = await manufacturerRepository.CreateAsync(name);

            Assert.NotNull(manufacturer);
            Assert.Equal(name, manufacturer.Name);

            // Find manufacturer
            ManufacturerEntity? retrieved = await manufacturerRepository.FindByNameAsync(name);

            Assert.NotNull(retrieved);
            Assert.Equal(manufacturer.Name, retrieved.Name);
            Assert.Equal(manufacturer.ManufacturerId, retrieved.ManufacturerId);

            await testContainer.DisposeAsync();
        }
    }
}