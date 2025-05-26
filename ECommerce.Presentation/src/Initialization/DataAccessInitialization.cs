using System.Data;
using ECommerce.Domain.Interfaces.Services;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Services.Payment;
using ECommerce.Infrastructure.Utils;

namespace ECommerce.Presentation.Initialization;

public static class DataAccessInitialization
{
    public static void InitDb(WebApplicationBuilder builder)
    {
        // EXTERNAL_DB is set when running on docker-compose. If thats the case, set host to db dns name
        string? host = Environment.GetEnvironmentVariable("EXTERNAL_DB") is null
            ? builder.Configuration.GetValue<string>("PostgreSQL:Host")
            : "db";

        var user = builder.Configuration.GetValue<string>("PostgreSQL:Username");
        var password = builder.Configuration.GetValue<string>("PostgreSQL:Password");
        var database = builder.Configuration.GetValue<string>("PostgreSQL:Database");

        if (host is null || user is null || password is null || database is null)
            throw new DataException(
                @"Configuration: PostgreSQL:Host, PostgreSQL:User, 
                PostgreSQL:Password, PostgreSQL:Database must be defined!"
            );

        builder.Services.AddSingleton<PostgresConfiguration>(_ =>
            new PostgresConfiguration(host, user, password, database));
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
        builder.Services.AddSingleton<IPaymentSessionRepository, PaymentSessionRepository>();
    }

    public static void InitStripe(WebApplicationBuilder builder)
    {
        string? stripeApiKey = builder.Configuration["STRIPE_API_KEY"];
        string? webhookSignature = builder.Configuration["STRIPE_WEBHOOK_SIGNATURE"];

        if (stripeApiKey is null) throw new DataException("Stripe api key is undefined!");

        if (webhookSignature is null) throw new DataException("Stripe webhook signature is undefined!");

        builder.Services.AddSingleton<StripeSettings>(_ => new StripeSettings
        {
            ApiKey = stripeApiKey,
            WebhookSignature = webhookSignature
        });

        builder.Services.AddSingleton<IPaymentSessionFactory, PaymentSessionFactory>();
    }
}