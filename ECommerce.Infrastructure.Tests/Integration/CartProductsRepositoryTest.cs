using System.Data;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Repositories.ProductStoreLocation;
using ECommerce.Infrastructure.Repositories.StoreLocation;
using ECommerce.Domain.Entities.CartProduct;
using ECommerce.Domain.Entities.Product;
using ECommerce.Domain.Entities.ProductStoreLocation;
using ECommerce.Domain.Entities.User;
using ECommerce.Domain.Models.CartProduct;
using ECommerce.Domain.Models.StoreLocation;
using ECommerce.TestUtils.TestDatabase;
using Microsoft.Extensions.Logging;
using Moq;

namespace DataAccess.Tests.Integration
{
    public class CartProductsRepositoryTest
    {
        [Fact]
        public async Task ShouldSuccesfullyCreateAndRetrieveACartProduct()
        {
            var testContainer = new TestDatabase();

            // Create user
            var userRepository = new UserRepository(testContainer._postgresService, new Mock<ILogger>().Object);
            var user = new UserEntity(Guid.NewGuid().ToString(), "email@gmail.com", "passwordhash", "firstname", "lastname");
            if (user is null)
            {
                throw new DataException("Failed to create the user");
            }
            await userRepository.CreateAsync(user);

            // Create manufacturer
            var manufacturerRepository = new ManufacturerRepository(testContainer._postgresService);
            var manufacturerDb = await manufacturerRepository.CreateAsync("Name");
            if (manufacturerDb is null)
            {
                throw new DataException("Failed to create a manufacturer");
            }


            // Create category
            var categoryRepository = new CategoryRepository(testContainer._postgresService);
            var categoryDb = await categoryRepository.CreateAsync("Name");
            if (categoryDb is null)
            {
                throw new DataException("Failed to create a product category");
            }


            // Create product
            var productRepository = new ProductRepository(testContainer._postgresService);
            var productDb = await productRepository.CreateAsync(new ProductEntity("Product name", "Product descriptino", 5.99m, 1, 1));
            if (productDb is null)
            {
                throw new DataException("Failed to create product");
            }

            // Create store location
            var storeLocationRepository = new StoreLocationRepository(testContainer._postgresService);
            var storeLocationDb = await storeLocationRepository.CreateAsync(new CreateStoreLocationModel("Display name", "address"));
            if (storeLocationDb is null)
            {
                throw new DataException("Failed to create a store location");
            }

            // Attach product to the store location
            var productStoreLocationRepository = new ProductStoreLocationRepository(testContainer._postgresService);
            await productStoreLocationRepository.AddProductToStore(
                new ProductStoreLocationEntity(storeLocationDb.StoreLocationId, productDb.ProductId, 5)
            );

            // Create cart product
            int itemQuantity = 2;
            var cartProductsRepository = new CartProductsRepository(testContainer._postgresService);
            await cartProductsRepository.AddItemAsync(
                new CartProductEntity(
                    user.UserId,
                    productDb.ProductId,
                    storeLocationDb.StoreLocationId,
                    itemQuantity
                )
            );

            // Get user cart products
            var items = await cartProductsRepository.GetUserCartProductsDetailedAsync(user.UserId);
            CartProductModel? targetItem = items.FirstOrDefault();

            Assert.NotNull(targetItem);
            Assert.Equal(productDb.ProductId, targetItem.ProductId);
            Assert.Equal(productDb.Price, targetItem.Price);
            Assert.Equal(itemQuantity, targetItem.Quantity);

            await testContainer.DisposeAsync();
        }
    }
}