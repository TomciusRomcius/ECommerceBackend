using ProductService.Application;
using ProductService.Application.Persistence;
using ProductService.Application.Services;
using ECommerceBackend.Utils.Database;
using ECommerceBackend.Utils.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddOptions<PostgresConfiguration>()
    .Bind(builder.Configuration.GetSection("Database"))
    .ValidateDataAnnotations();

builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(MediatREntryPoint).Assembly));
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddApplicationAuth(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.MapControllers();
app.UseHttpsRedirection();
app.UseApplicationAuth();

app.Run();