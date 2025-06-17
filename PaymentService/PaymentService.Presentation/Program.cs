using EventSystemHelper.Kafka.Utils;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Services;
using PaymentService.Domain.Enums;
using PaymentService.Infrastructure.Interfaces;
using PaymentService.Infrastructure.Services;
using PaymentService.Infrastructure.Utils;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddOptions<StripeSettings>()
    .Bind(builder.Configuration.GetSection("Stripe"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

string? kafkaServers = builder.Configuration.GetSection("Kafka").GetValue<string>("Servers");
ArgumentNullException.ThrowIfNull(kafkaServers);
var kafkaConfig = new KafkaConfiguration(kafkaServers);
builder.Services.AddSingleton<KafkaConfiguration>(kafkaConfig);
builder.Services.AddSingleton<WebhookEventStrategyMapContainer>();
builder.Services.AddSingleton<IWebhookCoordinatorService, StripeWebhookCoordinatorService>();
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddSingleton<IPaymentSessionFactory, PaymentSessionFactory>();

// Get all stripe webhook event handling strategies and register them to the DI container
Assembly? infrastructureAssembly = Assembly.GetAssembly(typeof(IStripeWebhookStrategy));
ArgumentNullException.ThrowIfNull(infrastructureAssembly);
IEnumerable<Type> validTypes = infrastructureAssembly.GetTypes()
    .Where(t => typeof(IStripeWebhookStrategy)
    .IsAssignableFrom(t)
    && // Only retrieve classes
    t is { IsClass: true, IsAbstract: false });

foreach (Type type in infrastructureAssembly.GetTypes().Where(t => typeof(IStripeWebhookStrategy).IsAssignableFrom(t) &&
    t is { IsClass: true, IsAbstract: false }))
{
    builder.Services.AddSingleton(typeof(IStripeWebhookStrategy), type);
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    WebhookEventStrategyMapContainer c = scope.ServiceProvider.GetRequiredService<WebhookEventStrategyMapContainer>();
    WebhookEventStrategyMap<IStripeWebhookStrategy> stripeStrategyMap = new WebhookEventStrategyMap<IStripeWebhookStrategy>();

    // Populate the stripe webhook strategy map with registered strategies
    var stripeStrategies = scope.ServiceProvider.GetServices<IStripeWebhookStrategy>();
    foreach (var strategy in stripeStrategies)
    {
        stripeStrategyMap.AddStrategy(strategy.EventType, strategy);
    }

    // Add strategy map to the strategy map container
    c.AddStrategyMap(PaymentProvider.STRIPE, stripeStrategyMap);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
