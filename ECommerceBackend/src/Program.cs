using ECommerce.Address;
using ECommerce.Cart;
using ECommerce.Categories;
using ECommerce.Common.Utils;
using ECommerce.DataAccess.Repositories;
using ECommerce.DataAccess.Repositories.ProductStoreLocation;
using ECommerce.DataAccess.Repositories.StoreLocation;
using ECommerce.DataAccess.Services;
using ECommerce.Identity;
using ECommerce.Manufacturers;
using ECommerce.Product;
using ECommerce.ProductStoreLocation;
using ECommerce.StoreLocation;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

// Create database service
string? connectionString = builder.Configuration.GetConnectionString("PostgreSQL");
if (connectionString == null)
{
    throw new ArgumentException("Connection string is empty");
}

builder.Services.AddSingleton<ILogger>(_ => LoggerManager.GetInstance().CreateLogger("ECommerceBackend"));
builder.Services.AddSingleton<IPostgresService, PostgresService>(_ => new PostgresService(connectionString));
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IRoleTypeRepository, RoleTypeRepository>();
builder.Services.AddSingleton<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<IManufacturerRepository, ManufacturerRepository>();
builder.Services.AddSingleton<ICartProductsRepository, CartProductsRepository>();
builder.Services.AddSingleton<IAddressRepository, AddressRepository>();
builder.Services.AddSingleton<IStoreLocationRepository, StoreLocationRepository>();
builder.Services.AddSingleton<IProductStoreLocationRepository, ProductStoreLocationRepository>();

// TODO: define issuer in appsettings
string issuer = "localhost";

// Auth setup
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Tokens.AuthenticatorIssuer = issuer;
})
    .AddRoles<ApplicationUserRole>()
    .AddUserStore<PostgresUserStore>()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddRoleStore<PostgresRoleStore>()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddRoleManager<RoleManager<ApplicationUserRole>>();

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
builder.Services.AddSingleton<ICartService, CartService>();
builder.Services.AddSingleton<IAddressService, AddressService>();
builder.Services.AddSingleton<IStoreLocationService, StoreLocationService>();
builder.Services.AddSingleton<IProductStoreLocationService, ProductStoreLocationService>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Create default roles and create master account
using (var scope = app.Services.CreateScope())
{
    var defaultRoles = new[] { "MERCHANT", "ADMINISTRATOR" };

    var roleManager = scope.ServiceProvider.GetService<RoleManager<ApplicationUserRole>>();
    if (roleManager is null)
    {
        throw new InvalidOperationException("Role manager is null!");
    }
    foreach (var role in defaultRoles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new ApplicationUserRole { Name = role });
        }
    }

    // TODO: use secrets for this
    var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

    if (userManager is null)
    {
        throw new InvalidOperationException("User manager is null!");
    }

    ApplicationUser? user = await userManager.FindByEmailAsync("masteruser@gmail.com");

    if (user is null)
    {
        user = new ApplicationUser()
        {
            Email = "masteruser@gmail.com",
            UserName = "masteruser@gmail.com",
            Firstname = "master",
            Lastname = "master"
        };

        var result = await userManager.CreateAsync(user, "Masterpassword.55");
        if (result.Errors.Count() > 0)
        {
            throw new Exception(result.Errors.ToString());
        }
    }

    if (!await userManager.IsInRoleAsync(user, "ADMINISTRATOR"))
    {
        await userManager.AddToRolesAsync(user, ["MERCHANT", "ADMINISTRATOR"]);
    }
}

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
await app.RunAsync();