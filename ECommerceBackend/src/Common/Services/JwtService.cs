using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using ECommerce.Common.Utils;

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
        private readonly JwtOptions _jwtOptions;

        public JwtService(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;

            if (_jwtOptions.SigningKey.Length < 16)
            {
                throw new ArgumentException("Secret key too short!");
            }

            SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        }

        public string CreateUserToken(int userId, string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Iss, _jwtOptions.Issuer),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_jwtOptions.ExpirationHours),
                SigningCredentials = this.SigningCredentials,
            };

            var token = TokenHandler.CreateToken(tokenDescriptor);

            return TokenHandler.WriteToken(token);
        }

        // public JwtSecurityToken ReadToken(string jwtToken)
        // {
        //     // TODO: add valid issr
        //     var validationParameters = new TokenValidationParameters()
        //     {
        //         ValidateIssuer = false,
        //         ValidateAudience = false,
        //         ValidateLifetime = false,
        //         IssuerSigningKey = SecurityKey
        //     };

        //     SecurityToken validatedToken;
        //     TokenHandler.ValidateToken(jwtToken, validationParameters, out validatedToken);
        //     return validatedToken as JwtSecurityToken;
        // }
    }
}