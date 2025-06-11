using Microsoft.AspNetCore.Identity;

namespace ECommerce.Presentation.Identity;

public class ApplicationUserRole : IdentityRole<int>
{
    public ApplicationUserRole()
    {
    }

    public ApplicationUserRole(string roleName) : base(roleName)
    {
    }
}