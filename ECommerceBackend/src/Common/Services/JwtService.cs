using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace ECommerce.Common.Services
{
    public interface IJwtService
    {
        public string CreateUserToken(int userId, string email);
    }

    public class JwtService : IJwtService
    {
        private readonly SigningCredentials SigningCredentials;
        private readonly JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();

        public JwtService(string secretKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        public string CreateUserToken(int userId, string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = this.SigningCredentials,
            };

            var token = TokenHandler.CreateToken(tokenDescriptor);

            return TokenHandler.WriteToken(token);
        }
    }
}