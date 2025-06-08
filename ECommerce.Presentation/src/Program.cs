using ECommerce.Application;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Services.Order;
using ECommerce.Presentation.Common.Services;
using ECommerce.Presentation.Common.Utils;
using ECommerce.Presentation.Initialization;
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

builder.Services.AddSingleton<ILogger>(_ => LoggerManager.GetInstance().CreateLogger("ECommerceBackend"));
builder.Services.AddSingleton<IObjectValidator, ObjectValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(MediatREntryPoint).Assembly));

// Add validators from Domain layer
builder.Services.AddValidatorsFromAssemblyContaining<UserEntity>(ServiceLifetime.Singleton);

builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<IoBackgroundTaskRunner>();

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

string? masterUserEmail = builder.Configuration["MASTER_USER_EMAIL"];
string? masterUserPassword = builder.Configuration["MASTER_USER_PASSWORD"];
await Initialization.CreateDefaultRolesAndMasterUser(app, builder.Configuration);
DataAccessInitialization.InitializeStripeWebhookStrategies(app);

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
await app.RunAsync();