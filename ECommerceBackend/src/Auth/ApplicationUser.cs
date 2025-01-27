using Microsoft.AspNetCore.Identity;

namespace ECommerce.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public ApplicationUser() { }
        public ApplicationUser(string id, string email, string passwordHash) : base()
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}