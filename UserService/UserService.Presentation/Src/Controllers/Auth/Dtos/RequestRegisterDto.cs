namespace UserService.Presentation.Controllers.Auth.dtos;

public class RequestRegisterDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required DateTime Birthday { get; set; }
}