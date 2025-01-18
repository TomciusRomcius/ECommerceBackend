using System.ComponentModel.DataAnnotations;

namespace ECommerce.Auth
{
    public class SignUpWithPasswordRequestDto
    {
        public required string Name { get; set; }
        public required string Lastname { get; set; }
        [EmailAddress()]
        public required string Email { get; set; }
        [Length(8, 50)]
        public required string Password { get; set; }
    }
}