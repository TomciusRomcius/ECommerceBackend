using ECommerce.Identity;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Initialization
{
    public static class Initialization
    {
        public static async Task CreateDefaultRolesAndMasterUser(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                await CreateDefaultRoles(scope);
                await CreateDefaultMasterUser(scope);
            }
        }

        private static async Task CreateDefaultRoles(IServiceScope scope)
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
        }

        private static async Task CreateDefaultMasterUser(IServiceScope scope)
        {
            // TODO: use secrets for this
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

            if (userManager is null)
            {
                throw new InvalidOperationException("User manager is null!");
            }

            ApplicationUser? user = await userManager.FindByEmailAsync("masteruser@gmail.com");

            if (user is null)
            {
                // Pasword gets set in CreateAsync
                user = new ApplicationUser("master", "master", "masteruser@gmail.com", "");

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
    }

}