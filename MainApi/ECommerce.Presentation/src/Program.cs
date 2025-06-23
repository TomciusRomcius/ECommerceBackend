using ECommerce.Application;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Application.Services.Consumers;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Services.Order;
using ECommerce.Presentation.Common.Services;
using ECommerce.Presentation.Common.Utils;
using ECommerce.Presentation.Initialization;
using EventSystemHelper.Kafka.Utils;
using FluentValidation;
using System.Reflection;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(setup =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string? xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    setup.IncludeXmlComments(xmlPath);
});

string? kafkaServers = builder.Configuration.GetSection("Kafka").GetValue<string>("Servers");
builder.Services.AddSingleton<KafkaConfiguration>(_ => new KafkaConfiguration(kafkaServers));

builder.Services.AddSingleton<ILogger>(_ => LoggerManager.GetInstance().CreateLogger("ECommerceBackend"));
builder.Services.AddSingleton<IObjectValidator, ObjectValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(MediatREntryPoint).Assembly));

// Add validators from Domain layer
builder.Services.AddValidatorsFromAssemblyContaining<UserEntity>(ServiceLifetime.Singleton);

builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<IoBackgroundTaskRunner>();
builder.Services.AddHostedService<ChargeSucceededConsumer>();

DataAccessInitialization.InitDb(builder);
DataAccessInitialization.InitRepositories(builder);
DataAccessInitialization.InitStripe(builder);

ServicesInitialization.InitializeServices(builder);
ServicesInitialization.InitializeIdentity(builder);

builder.Services.AddControllers();

WebApplication? app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

await Initialization.CreateDefaultRolesAndMasterUser(app);
DataAccessInitialization.InitializeStripeWebhookStrategies(app);

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
await app.RunAsync();