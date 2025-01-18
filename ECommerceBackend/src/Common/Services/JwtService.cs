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
        private readonly SymmetricSecurityKey SecurityKey;

        public JwtService(string secretKey)
        {
            if (secretKey.Length < 16)
            {
                throw new ArgumentException("Secret key too short!");
            }
            SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
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

        public JwtSecurityToken ReadToken(string jwtToken)
        {
            // TODO: add valid issr
            var validationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                IssuerSigningKey = SecurityKey
            };

            SecurityToken validatedToken;
            TokenHandler.ValidateToken(jwtToken, validationParameters, out validatedToken);
            return validatedToken as JwtSecurityToken;
        }
    }
}