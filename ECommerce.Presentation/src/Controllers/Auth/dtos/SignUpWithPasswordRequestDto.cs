using System.ComponentModel.DataAnnotations;

namespace ECommerce.Presentation.src.Controllers.Auth.dtos;

public class SignUpWithPasswordRequestDto
{
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }

    [EmailAddress] public required string Email { get; set; }

    [Length(8, 50)] public required string Password { get; set; }
}