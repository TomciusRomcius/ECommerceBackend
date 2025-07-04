using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.src.Repositories;
using ECommerce.Infrastructure.src.Services;

namespace ECommerce.Presentation.src.Initialization;

public static class DataAccessInitialization
{
    public static void InitDb(WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<PostgresConfiguration>()
            .Bind(builder.Configuration.GetSection("Database"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.Services.AddSingleton<IPostgresService, PostgresService>();
    }

    public static void InitRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IUserRepository, UserRepository>();
        builder.Services.AddSingleton<IRoleTypeRepository, RoleTypeRepository>();
        builder.Services.AddSingleton<IUserRoleRepository, UserRoleRepository>();
        builder.Services.AddSingleton<IProductRepository, ProductRepository>();
        builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
        builder.Services.AddSingleton<IManufacturerRepository, ManufacturerRepository>();
        builder.Services.AddSingleton<ICartProductsRepository, CartProductsRepository>();
        builder.Services.AddSingleton<IShippingAddressRepository, ShippingAddressRepository>();
        builder.Services.AddSingleton<IStoreLocationRepository, StoreLocationRepository>();
        builder.Services.AddSingleton<IProductStoreLocationRepository, ProductStoreLocationRepository>();
    }
}