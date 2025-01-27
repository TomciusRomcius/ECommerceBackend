namespace ECommerce.DataAccess.Models
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public UserModel(string userId, string email, string passwordHash, string firstname, string lastname)
        {
            UserId = userId;
            Email = email;
            PasswordHash = passwordHash;
            Firstname = firstname;
            Lastname = lastname;
        }
    }
}