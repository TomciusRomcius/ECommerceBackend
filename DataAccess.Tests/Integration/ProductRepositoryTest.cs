using DataAccess.Test.Integration.Utils;
using ECommerce.DataAccess.Models.Product;
using ECommerce.DataAccess.Models.User;
using ECommerce.DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace DataAccess.Test
{
    public class ProductRepositoryTest
    {
        [Fact]
        public async Task ShouldSuccesfullyCreateAndRetrieveTheProduct()
        {
            var testContainer = await TestContainerPostgresServiceWrapper.CreateAsync();
            var productRepository = new ProductRepository(testContainer._postgresService);
            var categoryRepository = new CategoryRepository(testContainer._postgresService);
            var manufacturerRepository = new ManufacturerRepository(testContainer._postgresService);

            string name = "New product name";
            string description = "New product description";
            double price = 5.99;
            int manufacturerId = 1;
            int categoryId = 1;

            await manufacturerRepository.CreateAsync("manufacturer");
            await categoryRepository.CreateAsync("category");

            await productRepository.CreateAsync(new ProductModel(name, description, price, manufacturerId, categoryId));

            var retrieved = await productRepository.FindByNameAsync(name);

            Assert.NotNull(retrieved);

            await testContainer.DisposeAsync();
        }
    }
}