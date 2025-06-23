using ECommerce.Application.src;
using ECommerce.Application.src.Interfaces;
using ECommerce.Application.src.Services;
using ECommerce.Application.src.Services.Consumers;
using ECommerce.Application.src.Utils;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Services.Order;
using ECommerce.Presentation.src.Common.Services;
using EventSystemHelper.Kafka.Utils;
using FluentValidation;
using Microsoft.Extensions.Options;
using System.Reflection;
using ECommerce.Presentation.Initialization;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(setup =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string? xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    setup.IncludeXmlComments(xmlPath);
});

builder.Services.AddHttpClient();

builder.Services.AddLogging();

// Not the best way, but currently the only way as KafkaConfiguration does not have a default constructor
string? kafkaServers = builder.Configuration.GetSection("Kafka")["Servers"];
if (String.IsNullOrWhiteSpace(kafkaServers)) throw new InvalidDataException("Kafka__Servers is not set!");
var kafkaConfiguration = new KafkaConfiguration(kafkaServers);
builder.Services.AddSingleton<IOptions<KafkaConfiguration>>(_ => Options.Create(kafkaConfiguration));

builder.Services.AddOptions<MicroserviceNetworkConfig>().Bind(builder.Configuration.GetSection("MicroserviceNetworkConfig"));
builder.Services.AddSingleton<IObjectValidator, ObjectValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(MediatREntryPoint).Assembly));

// Add validators from Domain layer
builder.Services.AddValidatorsFromAssemblyContaining<UserEntity>(ServiceLifetime.Singleton);

builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<IoBackgroundTaskRunner>();
builder.Services.AddHostedService<ChargeSucceededConsumer>();

builder.Services.Configure<MicroserviceNetworkConfig>(builder.Configuration.GetSection("MicroserviceNetworkConfig"));

DataAccessInitialization.InitDb(builder);

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

await DataAccessInitialization.WaitForConnectionAndAppliedMigrations(app);

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
await app.RunAsync();