namespace ECommerce.Domain.Models;

public class UpdateUserModel
{
    public UpdateUserModel(string userId, string? email, string? passwordHash, string? firstname, string? lastname)
    {
        UserId = userId;
        Email = email;
        PasswordHash = passwordHash;
        Firstname = firstname;
        Lastname = lastname;
    }

    public string UserId { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
}