using ECommerce.Domain.Entities;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Tests.Utils;
using TestUtils;

namespace ECommerce.Infrastructure.Tests.Integration.Repositories;

public class ManufacturerRepositoryTest
{
    [Fact]
    public async Task ShouldSuccesfullyCreateAndRetrieveManufacturer()
    {
        var testContainer = new TestDatabase();

        var manufacturerRepository = RepositoryFactories.CreateManufacturerRepository(testContainer._postgresService);
        var name = "manufacturer name";

        // Create manufacturer
        Result<int> manufacturerResult = await manufacturerRepository.CreateAsync(name);
        Assert.Empty(manufacturerResult.Errors);
        int manufacturerId = manufacturerResult.GetValue();

        // Find manufacturer
        ManufacturerEntity? retrieved = await manufacturerRepository.FindByNameAsync(name);

        Assert.NotNull(retrieved);
        Assert.Equal(name, retrieved.Name);
        Assert.Equal(manufacturerId, retrieved.ManufacturerId);

        await testContainer.DisposeAsync();
    }
}