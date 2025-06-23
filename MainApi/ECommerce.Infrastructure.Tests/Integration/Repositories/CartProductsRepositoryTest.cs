using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Tests.Utils;
using System.Data;
using TestUtils;

namespace ECommerce.Infrastructure.Tests.Integration.Repositories;

public class CartProductsRepositoryTest
{
    [Fact]
    public async Task ShouldSuccesfullyCreateAndRetrieveACartProduct()
    {
        var testContainer = new TestDatabase();
        PostgresService postgres = testContainer._postgresService;

        // Create user
        UserRepository userRepository = RepositoryFactories.CreateUserRepository(postgres);

        var user = new UserEntity(
            Guid.NewGuid().ToString(),
            "email@gmail.com",
            "passwordhash",
            "firstname",
            "lastname"
        );
        ResultError? userCreationError = await userRepository.CreateAsync(user);
        Assert.Null(userCreationError);

        // Create manufacturer
        ManufacturerRepository manufacturerRepository = RepositoryFactories.CreateManufacturerRepository(postgres);
        Result<int> manufacturerResult = await manufacturerRepository.CreateAsync("Name");
        Assert.Empty(manufacturerResult.Errors);

        // Create category
        CategoryRepository categoryRepository = RepositoryFactories.CreateCategoryRepository(postgres);
        Result<int> categoryResult = await categoryRepository.CreateAsync("Name");
        Assert.Empty(categoryResult.Errors);
        int categoryId = categoryResult.GetValue();

        // Create product
        ProductRepository productRepository = RepositoryFactories.CreateProductRepository(postgres);

        var productEntity = new ProductEntity(
            "Product name",
            "Product description",
            5.99m,
            1,
            categoryId
        );

        Result<ProductEntity> productResult = await productRepository.CreateAsync(productEntity);

        Assert.Empty(productResult.Errors);
        ProductEntity productDb = productResult.GetValue();

        // Create store location
        var storeLocationRepository = new StoreLocationRepository(testContainer._postgresService);
        StoreLocationEntity? storeLocationDb =
            await storeLocationRepository.CreateAsync(new CreateStoreLocationModel("Display name", "address"));
        if (storeLocationDb is null) throw new DataException("Failed to create a store location");

        // Attach product to the store location
        var productStoreLocationRepository = new ProductStoreLocationRepository(testContainer._postgresService);
        await productStoreLocationRepository.AddProductToStore(
            new ProductStoreLocationEntity(storeLocationDb.StoreLocationId, productDb.ProductId, 5)
        );

        // Create cart product
        const int itemQuantity = 2;
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
        Result<List<CartProductModel>> itemsResult = await cartProductsRepository.GetUserCartProductsDetailedAsync(user.UserId);

        Assert.Empty(itemsResult.Errors);
        List<CartProductModel> items = itemsResult.GetValue();
        CartProductModel? targetItem = items.FirstOrDefault();

        Assert.NotNull(targetItem);
        Assert.Equal(productDb.ProductId, targetItem.ProductId);
        Assert.Equal(productDb.Price, targetItem.Price);
        Assert.Equal(itemQuantity, targetItem.Quantity);

        await testContainer.DisposeAsync();
    }
}