using System.Data;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Tests.Utils;
using TestUtils;

/*
    Tests primarily product repository but also tests category and manufacturer repository CreateAsync methods
*/

namespace ECommerce.Infrastructure.Tests.Integration.Repositories;

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

        CategoryRepository categoryRepository = RepositoryFactories.CreateCategoryRepository(
            _container._postgresService
        );
        ManufacturerRepository manufacturerRepository = RepositoryFactories.CreateManufacturerRepository(
            _container._postgresService
        );
        ProductRepository productRepository = RepositoryFactories.CreateProductRepository(
            _container._postgresService
        );

        Result<int> manufacturerResult = await manufacturerRepository.CreateAsync("manufacturer");
        Assert.Empty(manufacturerResult.Errors);
        int manufacturerId = manufacturerResult.GetValue();

        Result<int> categoryResult = await categoryRepository.CreateAsync("category");
        Assert.Empty(categoryResult.Errors);
        int categoryId = categoryResult.GetValue();

        const string name = "New product name";
        const string description = "New product description";
        const decimal price = 5.99m;

        await productRepository.CreateAsync(new ProductEntity(name, description, price, manufacturerId, categoryId));

        ProductEntity? retrieved = await productRepository.FindByNameAsync(name);

        Assert.NotNull(retrieved);
        Assert.Equal(name, retrieved.Name);
        Assert.Equal(description, retrieved.Description);
        Assert.Equal(price, retrieved.Price);
    }
}