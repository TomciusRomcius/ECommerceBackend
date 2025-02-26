using ECommerce.Address;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Services;
using ECommerce.Categories;
using ECommerce.Domain.Interfaces.Services;
using ECommerce.Domain.Services;
using ECommerce.Identity;
using ECommerce.Manufacturers;
using ECommerce.ProductStoreLocation;
using ECommerce.StoreLocation;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Initialization
{
    public static class ServicesInitialization
    {
        public static void InitializeServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ICategoriesService, CategoriesService>();
            builder.Services.AddSingleton<IOrderService, OrderService>();
            builder.Services.AddSingleton<IAddressService, AddressService>();
            builder.Services.AddSingleton<IStoreLocationService, StoreLocationService>();
            builder.Services.AddSingleton<IProductStoreLocationService, ProductStoreLocationService>();

            // Validators
            builder.Services.AddSingleton<IOrderValidator, OrderValidator>();


            // Utils
            builder.Services.AddSingleton<IOrderPriceCalculator, OrderPriceCalculator>();
        }

        public static void InitializeIdentity(WebApplicationBuilder builder)
        {
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
        }
    }
}
