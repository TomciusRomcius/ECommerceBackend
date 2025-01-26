using ECommerce.Auth;
using ECommerce.Categories;
using ECommerce.Common.Services;
using ECommerce.Common.Utils;
using ECommerce.Manufacturers;
using ECommerce.Product;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

// Create database service
string? connectionString = builder.Configuration.GetConnectionString("PostgreSQL");
if (connectionString == null)
{
    throw new ArgumentException("Connection string is empty");
}

builder.Services.AddSingleton<IPostgresService, PostgresService>(_ => new PostgresService(connectionString));

// JWT service
var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
if (jwtOptions == null)
{
    throw new ArgumentException("Jwt options is null");
}

builder.Services.AddSingleton<JwtOptions>(jwtOptions);
builder.Services.AddSingleton<IJwtService, JwtService>(_ => new JwtService(jwtOptions));

// Auth setup
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Tokens.AuthenticatorIssuer = jwtOptions.Issuer;
})
    .AddUserStore<PostgresUserStore>()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddUserManager<UserManager<ApplicationUser>>();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie("Identity.Application", o =>
    {
        o.Cookie.Name = "user";
        o.Cookie.Domain = "localhost";
        o.SlidingExpiration = true;
        o.ExpireTimeSpan = TimeSpan.FromDays(30);
    });

// Services that controllers depend on
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<ICategoriesService, CategoriesService>();
builder.Services.AddSingleton<IManufacturerService, ManufacturerService>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();