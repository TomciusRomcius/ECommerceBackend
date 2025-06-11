using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

public class UserEntity
{
    public UserEntity(string userId, string email, string passwordHash, string firstname, string lastname)
    {
        UserId = userId;
        Email = email;
        PasswordHash = passwordHash;
        Firstname = firstname;
        Lastname = lastname;
    }

    [Required(AllowEmptyStrings = false, ErrorMessage = "User id cannot be empty")]
    public string UserId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Email cannot be empty")]
    public string Email { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Password hash cannot be empty")]
    public string PasswordHash { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Firstname cannot be empty")]
    public string Firstname { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Lastname cannot be empty")]
    public string Lastname { get; set; }
}