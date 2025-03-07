namespace ECommerce.Domain.Models.User
{
    public class UpdateUserModel
    {
        public string UserId { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }

        public UpdateUserModel(string userId, string? email, string? passwordHash, string? firstname, string? lastname)
        {
            UserId = userId;
            Email = email;
            PasswordHash = passwordHash;
            Firstname = firstname;
            Lastname = lastname;
        }
    }
}