using System.Data;
using ECommerce.TestUtils.TestDatabase;
using ECommerce.DataAccess.Models.Product;
using ECommerce.DataAccess.Repositories;

/*
    Tests primarily product repository but also tests category and manufacturer repository CreateAsync methods
*/

namespace DataAccess.Tests.Integration
{
    [Collection("Product operations")]
    public class ProductCreationFlowTest : IClassFixture<TestDatabase>
    {
        TestDatabase _container;

        public ProductCreationFlowTest(TestDatabase container)
        {
            _container = container;
        }

        [Fact]
        public async Task ShouldSuccesfullyCreateAndRetrieveTheProduct()
        {
            if (_container is null)
            {
                throw new DataException("Test container is null");
            }

            var categoryRepository = new CategoryRepository(_container._postgresService);
            var manufacturerRepository = new ManufacturerRepository(_container._postgresService);
            var productRepository = new ProductRepository(_container._postgresService);

            int manufacturerId = (await manufacturerRepository.CreateAsync("manufacturer"))!.ManufacturerId;
            int categoryId = (await categoryRepository.CreateAsync("category"))!.CategoryId;

            string name = "New product name";
            string description = "New product description";
            double price = 5.99;

            await productRepository.CreateAsync(new ProductModel(name, description, price, manufacturerId, categoryId));

            var retrieved = await productRepository.FindByNameAsync(name);

            Assert.NotNull(retrieved);
            Assert.Equal(name, retrieved.Name);
            Assert.Equal(description, retrieved.Description);
            Assert.Equal(price, retrieved.Price);
        }
    }
}