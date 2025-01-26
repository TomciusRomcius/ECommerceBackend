using Microsoft.AspNetCore.Identity;

namespace ECommerce.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }
        public ApplicationUser(string id, string email, string passwordHash) : base()
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}