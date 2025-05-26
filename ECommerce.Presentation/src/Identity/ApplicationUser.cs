using Microsoft.AspNetCore.Identity;

namespace ECommerce.Presentation.Identity;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser(string id, string firstname, string lastname, string email, string passwordHash)
    {
        UserName = email;
        Id = id;
        Firstname = firstname;
        Lastname = lastname;
        Email = email;
        PasswordHash = passwordHash;
    }

    public ApplicationUser(string firstname, string lastname, string email, string passwordHash)
    {
        UserName = email;
        Firstname = firstname;
        Lastname = lastname;
        Email = email;
        PasswordHash = passwordHash;
    }

    public string Firstname { get; set; }
    public string Lastname { get; set; }
}