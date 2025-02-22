using Microsoft.AspNetCore.Identity;

namespace ECommerce.Identity
{
    public class ApplicationUserRole : IdentityRole<int>
    {

        public ApplicationUserRole() { }

        public ApplicationUserRole(string roleName) : base(roleName)
        {
        }
    }
}