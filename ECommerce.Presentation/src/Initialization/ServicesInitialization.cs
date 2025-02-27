using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Services;
using ECommerce.Categories;
using ECommerce.Domain.Services;
using ECommerce.Identity;
using ECommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Initialization
{
    public static class ServicesInitialization
    {
        public static void InitializeServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ICategoriesService, CategoriesService>();
            builder.Services.AddSingleton<IOrderService, OrderService>();
            builder.Services.AddSingleton<IOrderValidator, OrderValidator>();
            builder.Services.AddSingleton<IOrderPriceCalculator, OrderPriceCalculator>();
        }

        public static void InitializeIdentity(WebApplicationBuilder builder, OpenIdProviderConfigService openIdProviderConfigService)
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

            var authBuilder = builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddCookie("Identity.Application", options =>
                {
                    options.Cookie.Name = "user";
                    options.Cookie.Domain = "localhost";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                });

            // Add openid providers
            foreach (var instance in openIdProviderConfigService._providers)
            {
                var provider = instance.Value;

                authBuilder.AddOpenIdConnect(options =>
                {
                    options.Authority = provider.Authority;
                    options.ClientId = provider.ClientId;
                    options.ClientSecret = provider.ClientSecret;
                });
            }
        }
    }
}
