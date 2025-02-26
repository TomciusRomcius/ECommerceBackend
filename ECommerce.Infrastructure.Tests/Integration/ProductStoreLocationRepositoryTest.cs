using ECommerce.TestUtils.TestDatabase;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Repositories.ProductStoreLocation;
using ECommerce.Infrastructure.Repositories.StoreLocation;
using ECommerce.Domain.Entities.StoreLocation;
using ECommerce.Domain.Entities.Manufacturer;
using ECommerce.Domain.Entities.Category;
using ECommerce.Domain.Entities.Product;
using ECommerce.Domain.Models.StoreLocation;
using ECommerce.Domain.Models.ProductStoreLocation;
using ECommerce.Domain.Entities.ProductStoreLocation;
using ECommerce.Domain.Entities.CartProduct;

namespace DataAccess.Tests.Integration
{
    public class ProductStoreLocationRepositoryTest
    {
        private async Task<(StoreLocationEntity, ProductEntity)> CreateTestStoreLocationAndProductModel(TestDatabase testContainer)
        {
            ManufacturerEntity? manufacturer = await new ManufacturerRepository(testContainer._postgresService).CreateAsync("Manufacturer");
            CategoryEntity? category = await new CategoryRepository(testContainer._postgresService).CreateAsync("Category");

            ProductEntity? product = await new ProductRepository(testContainer._postgresService).CreateAsync(
                new ProductEntity("Name", "Description", 2.99m, manufacturer!.ManufacturerId, category!.CategoryId
            ));

            StoreLocationEntity? storeLocation = await new StoreLocationRepository(testContainer._postgresService).CreateAsync(
                new CreateStoreLocationModel("Display name", "Address")
            );

            return (storeLocation, product);
        }

        [Fact]
        public async Task ShouldSuccesfullyCreateProductStoreLocationEntries()
        {
            var testContainer = new TestDatabase();

            (StoreLocationEntity, ProductEntity) testData = await CreateTestStoreLocationAndProductModel(testContainer);

            var productStoreLocationRepository = new ProductStoreLocationRepository(testContainer._postgresService);

            int stock = 5;

            await productStoreLocationRepository.AddProductToStore(
                new ProductStoreLocationEntity(testData.Item1.StoreLocationId, testData.Item2.ProductId, stock)
            );

            DetailedProductModel? retrieved = (await productStoreLocationRepository.GetProductsFromStoreAsync(testData.Item1.StoreLocationId)).FirstOrDefault();

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

            int stock = 5;

            await productStoreLocationRepository.AddProductToStore(
                new ProductStoreLocationEntity(testData.Item1.StoreLocationId, testData.Item2.ProductId, stock)
            );

            int quantity = 3;

            await productStoreLocationRepository.UpdateStock([
                new CartProductEntity(Guid.NewGuid().ToString(), testData.Item2.ProductId, testData.Item1.StoreLocationId, quantity)
            ]);

            DetailedProductModel? retrieved = (await productStoreLocationRepository.GetProductsFromStoreAsync(testData.Item1.StoreLocationId)).FirstOrDefault();

            Assert.NotNull(retrieved);
            Assert.Equal(stock - quantity, retrieved.Stock);

            await testContainer.DisposeAsync();
        }
    }
}