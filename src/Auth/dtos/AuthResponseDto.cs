namespace ECommerce.Auth
{
    public class AuthResponseDto
    {
        public required string jwtToken { get; set; }
        public required int userId { get; set; }
    }
}