using System.Data;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Repositories;
using TestUtils;

/*
    Tests primarily product repository but also tests category and manufacturer repository CreateAsync methods
*/

namespace ECommerce.Infrastructure.Tests.Integration;

[Collection("Product operations")]
public class ProductCreationFlowTest : IClassFixture<TestDatabase>
{
    private readonly TestDatabase _container;

    public ProductCreationFlowTest(TestDatabase container)
    {
        _container = container;
    }

    [Fact]
    public async Task ShouldSuccesfullyCreateAndRetrieveTheProduct()
    {
        if (_container is null) throw new DataException("Test container is null");

        var categoryRepository = new CategoryRepository(_container._postgresService);
        var manufacturerRepository = new ManufacturerRepository(_container._postgresService);
        var productRepository = new ProductRepository(_container._postgresService);

        int manufacturerId = (await manufacturerRepository.CreateAsync("manufacturer"))!.ManufacturerId;
        int categoryId = (await categoryRepository.CreateAsync("category"))!.CategoryId;

        var name = "New product name";
        var description = "New product description";
        var price = 5.99m;

        await productRepository.CreateAsync(new ProductEntity(name, description, price, manufacturerId, categoryId));

        ProductEntity? retrieved = await productRepository.FindByNameAsync(name);

        Assert.NotNull(retrieved);
        Assert.Equal(name, retrieved.Name);
        Assert.Equal(description, retrieved.Description);
        Assert.Equal(price, retrieved.Price);
    }
}