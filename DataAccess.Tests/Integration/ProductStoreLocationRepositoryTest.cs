using ECommerce.TestUtils.TestDatabase;
using ECommerce.DataAccess.Models.RoleType;
using ECommerce.DataAccess.Repositories;
using ECommerce.DataAccess.Repositories.ProductStoreLocation;
using ECommerce.DataAccess.Models.Product;
using ECommerce.DataAccess.Models.Manufacturer;
using ECommerce.DataAccess.Models.Category;
using ECommerce.DataAccess.Models.ProductStoreLocation;
using ECommerce.DataAccess.Models.StoreLocation;
using ECommerce.DataAccess.Repositories.StoreLocation;
using ECommerce.DataAccess.Models.CartProduct;
using ECommerce.DataAccess.Entities.CartProduct;

namespace DataAccess.Tests.Integration
{
    public class ProductStoreLocationRepositoryTest
    {
        private async Task<(StoreLocationModel, ProductModel)> CreateTestStoreLocationAndProductModel(TestDatabase testContainer)
        {
            ManufacturerModel? manufacturer = await new ManufacturerRepository(testContainer._postgresService).CreateAsync("Manufacturer");
            CategoryModel? category = await new CategoryRepository(testContainer._postgresService).CreateAsync("Category");

            ProductModel? product = await new ProductRepository(testContainer._postgresService).CreateAsync(
                new ProductModel("Name", "Description", 2.99, manufacturer!.ManufacturerId, category!.CategoryId
            ));

            StoreLocationModel? storeLocation = await new StoreLocationRepository(testContainer._postgresService).CreateAsync(
                new CreateStoreLocationModel("Display name", "Address")
            );

            return (storeLocation, product);
        }

        [Fact]
        public async Task ShouldSuccesfullyCreateProductStoreLocationEntries()
        {
            var testContainer = new TestDatabase();

            (StoreLocationModel, ProductModel) testData = await CreateTestStoreLocationAndProductModel(testContainer);

            var productStoreLocationRepository = new ProductStoreLocationRepository(testContainer._postgresService);

            int stock = 5;

            await productStoreLocationRepository.AddProductToStore(
                new ProductStoreLocationModel(testData.Item1.StoreLocationId, testData.Item2.ProductId, stock)
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

            (StoreLocationModel, ProductModel) testData = await CreateTestStoreLocationAndProductModel(testContainer);

            var productStoreLocationRepository = new ProductStoreLocationRepository(testContainer._postgresService);

            int stock = 5;

            await productStoreLocationRepository.AddProductToStore(
                new ProductStoreLocationModel(testData.Item1.StoreLocationId, testData.Item2.ProductId, stock)
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