using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Domain.Services.Order;
using ECommerce.Presentation.Identity;
using ECommerce.Presentation.src.Controllers.Categories;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Presentation.Initialization;

public static class ServicesInitialization
{
    public static void InitializeServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ICategoriesService, CategoriesService>();
        builder.Services.AddSingleton<IOrderService, OrderService>();
        builder.Services.AddSingleton<IOrderValidator, OrderValidator>();
        builder.Services.AddSingleton<IOrderPriceCalculator, OrderPriceCalculator>();
    }

    public static void InitializeIdentity(WebApplicationBuilder builder)
    {
        // TODO: define issuer in appsettings
        var issuer = "localhost";

        // Auth setup
        builder.Services.AddIdentityCore<ApplicationUser>(options => { options.Tokens.AuthenticatorIssuer = issuer; })
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