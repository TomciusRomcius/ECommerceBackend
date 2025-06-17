using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Application.Services.WebhookStrategies;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Interfaces.Services;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Interfaces;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Services.Payment;
using ECommerce.Infrastructure.Utils;
using Stripe;
using System.Data;

namespace ECommerce.Presentation.Initialization;

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
        builder.Services.AddSingleton<WebhookEventStrategyMapContainer>();
        builder.Services.AddSingleton<IWebhookCoordinatorService, StripeWebhookCoordinatorService>();
    }

    public static void InitializeStripeWebhookStrategies(WebApplication app)
    {
        IWebhookEventStrategyMap stripeStrategyMap = new WebhookEventStrategyMap<IStripeWebhookStrategy>();

        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            var strategyMapContainer = serviceProvider.GetRequiredService<WebhookEventStrategyMapContainer>();
            // Get required services
            IOrderService orderService = serviceProvider.GetRequiredService<IOrderService>();

            // Create strategies
            IStripeWebhookStrategy chargeSucceeded = new ChargeSucceededStrategy(orderService);

            // Add strategies to stripe
            stripeStrategyMap.AddStrategy<IStripeWebhookStrategy>(EventTypes.ChargeSucceeded, chargeSucceeded);

            strategyMapContainer.AddStrategyMap(PaymentProvider.STRIPE, stripeStrategyMap);
        }


    }
}