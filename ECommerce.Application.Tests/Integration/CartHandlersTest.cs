using ECommerce.Application.src.UseCases.Cart.Commands;
using ECommerce.Application.src.UseCases.Cart.Handlers;
using ECommerce.Application.Tests.Utils;
using ECommerce.Domain.src.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Tests.Integration
{
    public class CartHandlersTest : DbContextWithDependencyInjection
    {
        public CartHandlersTest() : base() { }

        protected override void PreServiceProviderCreation(IServiceCollection services)
        {
            services.AddScoped<AddItemToCartHandler>();
        }

        [Fact]
        public async Task Handle_ShouldAddAnItemToUsersCart()
        {
            // Arrange
            AddItemToCartHandler handler = ServiceProvider.GetRequiredService<AddItemToCartHandler>();

            var category = new CategoryEntity("Category");
            var manufacturer = new ManufacturerEntity("Manufacturer");

            await DbContext.AddRangeAsync([category, manufacturer]);
            await DbContext.SaveChangesAsync();

            var user = new IdentityUser("User");
            var product = new ProductEntity(
                "Product",
                "Description",
                2.99m,
                manufacturer.ManufacturerId,
                category.CategoryId
            );

            var storeLocation = new StoreLocationEntity("Display name", "Address");
            await DbContext.AddRangeAsync(product, storeLocation, user);
            await DbContext.SaveChangesAsync();
            var productStoreLocation = new ProductStoreLocationEntity(
                storeLocation.StoreLocationId,
                product.ProductId,
                5
            );
            await DbContext.AddAsync(productStoreLocation);
            await DbContext.SaveChangesAsync();

            var cartProduct = new CartProductEntity(
                user.Id,
                product.ProductId,
                storeLocation.StoreLocationId,
                5
            );

            await DbContext.AddAsync(cartProduct);
            await DbContext.SaveChangesAsync();

            // Act
            await handler.Handle(new AddItemToCartCommand(cartProduct), CancellationToken.None);

            // Assert
            CartProductEntity? retrieved = DbContext.CartProducts
                .Where(cp => cp.UserId == user.Id
                    && cp.ProductId == product.ProductId
                    && cp.StoreLocationId == productStoreLocation.StoreLocationId)
                .FirstOrDefault();

            Assert.NotNull(retrieved);
            Assert.Equal(cartProduct.Quantity, retrieved.Quantity);
        }
    }
}
