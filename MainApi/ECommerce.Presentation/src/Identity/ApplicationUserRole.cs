using Microsoft.AspNetCore.Identity;

namespace ECommerce.Presentation.src.Identity;

public class ApplicationUserRole : IdentityRole<int>
{
    public ApplicationUserRole()
    {
    }

    public ApplicationUserRole(string roleName) : base(roleName)
    {
    }
}