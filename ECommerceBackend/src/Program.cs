using ECommerce.Categories;
using ECommerce.Common.Utils;
using ECommerce.DataAccess.Repositories;
using ECommerce.DataAccess.Services;
using ECommerce.Identity;
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

builder.Services.AddSingleton<ILogger>(_ => LoggerManager.GetInstance().CreateLogger("ECommerceBackend"));
builder.Services.AddSingleton<IPostgresService, PostgresService>(_ => new PostgresService(connectionString));
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IRoleTypeRepository, RoleTypeRepository>();
builder.Services.AddSingleton<IUserRoleRepository, UserRoleRepository>();

// TODO: define issuer in appsettings
string issuer = "localhost";

// Auth setup

builder.Services.AddTransient<IUserStore<ApplicationUser>, PostgresUserStore>();
builder.Services.AddTransient<IUserPasswordStore<ApplicationUser>, PostgresUserStore>();
builder.Services.AddTransient<IUserEmailStore<ApplicationUser>, PostgresUserStore>();
builder.Services.AddTransient<IUserRoleStore<ApplicationUser>, PostgresUserStore>();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Tokens.AuthenticatorIssuer = issuer;
})
    .AddUserStore<PostgresUserStore>()
    .AddRoles<ApplicationUserRole>()
    .AddRoleStore<PostgresRoleStore>()
    .AddRoleManager<RoleManager<ApplicationUserRole>>()
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

// Create default roles
using (var scope = app.Services.CreateScope())
{
    var defaultRoles = new[] { "Client", "Product manager", "Administrator" };

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
}


app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();