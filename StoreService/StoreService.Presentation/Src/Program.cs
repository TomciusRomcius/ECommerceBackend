using System.Data;
using ECommerceBackend.Utils.Database;
using Microsoft.Extensions.Options;
using StoreService.Application;
using StoreService.Application.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

if (builder.Configuration.GetSection("Database").Exists())
    builder.Services.AddOptions<PostgresConfiguration>()
        .Bind(builder.Configuration.GetSection("Database"))
        .ValidateDataAnnotations();

else if (builder.Configuration["ASPNETCORE_ENVIRONMENT"] != "Production")
    builder.Services.AddSingleton<IOptions<PostgresConfiguration>>(Options.Create<PostgresConfiguration>(
        new PostgresConfiguration
        {
            Host = "postgres",
            Database = "postgres",
            Username = "postgres",
            Password = "postgres"
        }));

else
    throw new DataException("Configuration Database properties are not defined!");

builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(MediatREntryPoint).Assembly));

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.Run();