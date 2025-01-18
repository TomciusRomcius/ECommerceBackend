using System.ComponentModel.DataAnnotations;

namespace ECommerce.Auth
{
    public class SignInWithPasswordRequestDto
    {
        [EmailAddress()]
        public required string Email { get; set; }
        [Length(8, 50)]
        public required string Password { get; set; }
    }
}