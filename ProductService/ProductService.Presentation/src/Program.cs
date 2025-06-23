using System.Data;
using Microsoft.Extensions.Options;
using ProductService.Application;
using ProductService.Application.Persistence;
using ProductService.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

if (builder.Configuration.GetSection("Database").Exists())
    builder.Services.AddOptions<PostgresConfiguration>()
        .Bind(builder.Configuration.GetSection("Database"))
        .ValidateDataAnnotations();

else if (builder.Configuration["ASPNETCORE_ENVIRONMENT"] != "Production")
    builder.Services.AddSingleton<IOptions<PostgresConfiguration>>(Options.Create<PostgresConfiguration>(
        new PostgresConfiguration
        {
            Host = "postgresas",
            Database = "postgres",
            Username = "postgres",
            Password = "postgres"
        }));

else
    throw new DataException("Configuration Database properties are not defined!");

builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(MediatREntryPoint).Assembly));
builder.Services.AddScoped<ICategoriesService, CategoriesService>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.MapControllers();
app.UseHttpsRedirection();

app.Run();