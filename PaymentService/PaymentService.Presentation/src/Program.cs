using EventSystemHelper.Kafka.Utils;
using Microsoft.Extensions.Options;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Persistence;
using PaymentService.Application.Services;
using PaymentService.Application.Utils;
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

if (builder.Configuration.GetSection("Database") != null)
{
    builder.Services.AddOptions<PostgresConfiguration>()
        .Bind(builder.Configuration.GetSection("Database"));
}
else
{
    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
    {
        throw new InvalidDataException("Database is not configured.");
    }
    builder.Services.AddSingleton<IOptions<PostgresConfiguration>>(Options.Create<PostgresConfiguration>(new PostgresConfiguration
    {
        Host = "",
        Database = "",
        Username = "",
        Password = ""
    }));
}

builder.Services.AddDbContext<DatabaseContext>();

string? kafkaServers = builder.Configuration.GetSection("Kafka")["Servers"];
if (String.IsNullOrWhiteSpace(kafkaServers))
{
    throw new InvalidDataException("Kafka__Servers not defined");
}

// Cant currently use normal AddOptions as KafkaConfiguration does not have a default constructor
builder.Services.AddSingleton(Options.Create(new KafkaConfiguration(
    kafkaServers
)));
builder.Services.AddOptions<KafkaConfiguration>()
    .Bind(builder.Configuration.GetSection("Kafka"))
    .ValidateDataAnnotations();
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
