using Microsoft.AspNetCore.Identity;

namespace ECommerce.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public ApplicationUser(string id, string firstname, string lastname, string email, string passwordHash) : base()
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            PasswordHash = passwordHash;
        }

        public ApplicationUser(string firstname, string lastname, string email, string passwordHash) : base()
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            PasswordHash = passwordHash;
        }

    }
}