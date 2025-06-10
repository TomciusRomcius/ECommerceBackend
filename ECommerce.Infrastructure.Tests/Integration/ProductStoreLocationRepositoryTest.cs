using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Tests.Utils;
using TestUtils;

namespace ECommerce.Infrastructure.Tests.Integration;

public class ProductStoreLocationRepositoryTest
{
    private async Task<(StoreLocationEntity, ProductEntity)> CreateTestStoreLocationAndProductModel(
        TestDatabase testContainer)
    {
        CategoryRepository categoryRepository = RepositoryFactories.CreateCategoryRepository(
            testContainer._postgresService
        );
        ManufacturerRepository manufacturerRepository = RepositoryFactories.CreateManufacturerRepository(
            testContainer._postgresService
        );

        Result<int> manufacturerResult = await manufacturerRepository.CreateAsync("Manufacturer");
        Assert.Empty(manufacturerResult.Errors);
        int manufacturerId = manufacturerResult.GetValue();
        
        Result<int> categoryResult = await categoryRepository.CreateAsync("Category");
        Assert.Empty(categoryResult.Errors);

        int categoryId = categoryResult.GetValue();
        
        var productEntity = new ProductEntity(
            "Product Name", "Description", 2.99m, manufacturerId, categoryId
        );

        ProductRepository productRepository = RepositoryFactories.CreateProductRepository(
            testContainer._postgresService
        );

        Result<ProductEntity> productResult= await productRepository.CreateAsync(productEntity);
        Assert.Empty(productResult.Errors);
        var product = productResult.GetValue();

        StoreLocationEntity? storeLocation =
            await new StoreLocationRepository(testContainer._postgresService).CreateAsync(
                new CreateStoreLocationModel("Display name", "Address")
            );

        Assert.NotNull(storeLocation);
        return (storeLocation, product);
    }

    [Fact]
    public async Task ShouldSuccesfullyCreateProductStoreLocationEntries()
    {
        var testContainer = new TestDatabase();

        (StoreLocationEntity, ProductEntity) testData = await CreateTestStoreLocationAndProductModel(testContainer);

        var productStoreLocationRepository = new ProductStoreLocationRepository(testContainer._postgresService);

        var stock = 5;

        await productStoreLocationRepository.AddProductToStore(
            new ProductStoreLocationEntity(testData.Item1.StoreLocationId, testData.Item2.ProductId, stock)
        );

        DetailedProductModel? retrieved =
            (await productStoreLocationRepository.GetProductsFromStoreAsync(testData.Item1.StoreLocationId))
            .FirstOrDefault();

        Assert.NotNull(retrieved);
        Assert.Equal(stock, retrieved.Stock);

        await testContainer.DisposeAsync();
    }

    [Fact]
    public async Task ShouldSuccesfullyCreateProductStoreLocationEntriesAndUpdateStock()
    {
        var testContainer = new TestDatabase();

        (StoreLocationEntity, ProductEntity) testData = await CreateTestStoreLocationAndProductModel(testContainer);

        var productStoreLocationRepository = new ProductStoreLocationRepository(testContainer._postgresService);

        var stock = 5;

        await productStoreLocationRepository.AddProductToStore(
            new ProductStoreLocationEntity(testData.Item1.StoreLocationId, testData.Item2.ProductId, stock)
        );

        var quantity = 3;

        var cartProductEntity = new CartProductEntity(
            Guid.NewGuid().ToString(), 
            testData.Item2.ProductId,
            testData.Item1.StoreLocationId,
            quantity);
        
        await productStoreLocationRepository.UpdateStock([
            cartProductEntity,
        ]);

        DetailedProductModel? retrieved =
            (await productStoreLocationRepository.GetProductsFromStoreAsync(testData.Item1.StoreLocationId))
            .FirstOrDefault();

        Assert.NotNull(retrieved);
        Assert.Equal(stock - quantity, retrieved.Stock);

        await testContainer.DisposeAsync();
    }
}