using ECommerce.Application.src.Interfaces;
using ECommerce.Application.src.Services;
using ECommerce.Domain.src.Interfaces.Services;
using ECommerce.Domain.src.Services.Order;
using ECommerce.Infrastructure.src.Services;
using ECommerce.Persistence;
using ECommerce.Persistence.src;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ECommerce.Presentation.Initialization;

public static class ServicesInitialization
{
    public static void InitializeServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IOrderService, OrderService>();
        builder.Services.AddTransient<IOrderPriceCalculator, OrderPriceCalculator>();
        builder.Services.AddTransient<IPaymentSessionService, PaymentSessionService>();
    }

    public static void InitializeIdentity(WebApplicationBuilder builder)
    {
        // TODO: Setup issuer correctly
        builder.Services.AddIdentityCore<IdentityUser>()
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddRoles<IdentityRole>()
            .AddUserStore<UserStore<IdentityUser, IdentityRole, DatabaseContext>>()
            .AddRoleStore<RoleStore<IdentityRole, DatabaseContext>>()
            .AddUserManager<UserManager<IdentityUser>>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddSignInManager<SignInManager<IdentityUser>>();

        builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddCookie(IdentityConstants.ApplicationScheme, o =>
            {
                o.Cookie.Name = "user";
                o.Cookie.Domain = "localhost";
                o.SlidingExpiration = true;
                o.ExpireTimeSpan = TimeSpan.FromDays(30);
            });
    }
}