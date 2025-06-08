using System.ComponentModel.DataAnnotations;

namespace ECommerce.Presentation.src.Controllers.Auth.dtos;

public class SignInWithPasswordRequestDto
{
    [EmailAddress] public required string Email { get; set; }

    [Length(8, 50)] public required string Password { get; set; }
}