using ECommerce.Presentation.Identity;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Presentation.Initialization;

public static class Initialization
{
    public static async Task CreateDefaultRolesAndMasterUser(WebApplication app, ConfigurationManager configuration)
    {
        using (IServiceScope? scope = app.Services.CreateScope())
        {
            string? email = configuration["MASTER_USER_EMAIL"];
            string? password = configuration["MASTER_USER_PASSWORD"];

            if (email is null) throw new ArgumentNullException("Master user email cannot be null");
            if (password is null) throw new ArgumentNullException("Master user password cannot be null");

            await CreateDefaultRoles(scope);
            await CreateDefaultMasterUser(scope, email, password);
        }
    }

    private static async Task CreateDefaultRoles(IServiceScope scope)
    {
        var defaultRoles = new[] { "MERCHANT", "ADMINISTRATOR" };

        var roleManager = scope.ServiceProvider.GetService<RoleManager<ApplicationUserRole>>();

        if (roleManager is null) throw new InvalidOperationException("Role manager is null!");

        foreach (string role in defaultRoles)
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new ApplicationUserRole { Name = role });
    }

    private static async Task CreateDefaultMasterUser(IServiceScope scope, string email, string password)
    {
        // TODO: use secrets for this
        var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

        if (userManager is null) throw new InvalidOperationException("User manager is null!");

        ApplicationUser? user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            // Pasword gets set in CreateAsync
            user = new ApplicationUser("master", "master", email, "");

            IdentityResult result = await userManager.CreateAsync(user, password);
            if (result.Errors.Count() > 0) throw new Exception(result.Errors.ToString());
        }

        if (!await userManager.IsInRoleAsync(user, "ADMINISTRATOR"))
            await userManager.AddToRolesAsync(user, ["MERCHANT", "ADMINISTRATOR"]);
    }
}