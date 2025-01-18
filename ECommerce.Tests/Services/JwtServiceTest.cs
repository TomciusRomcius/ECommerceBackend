using ECommerce.Common.Services;
using Xunit;

namespace ECommerce.Tests
{
    public class JwtServiceTest
    {
        [Fact]
        public void JwtService_ShouldCreateJwtTokens()
        {
            string key = "8E181A76C50E956FB62F3B620945D3BA75DF1E0A3A1B8C34CDD72FFB562379C4";
            int jwtPartsCount = 3;

            var jwtService = new JwtService(key);
            string result = jwtService.CreateUserToken(5, "email");
            string[] jwtParts = result.Split('.');

            Assert.NotEmpty(result);
            Assert.Equal(jwtParts.Length, jwtPartsCount);
        }

        [Fact]
        public void JwtServiceConstructor_ShouldThrowAnError_IfSecretKeyIsInvalid()
        {
            Assert.ThrowsAny<Exception>(() => new JwtService("abc"));
            Assert.ThrowsAny<Exception>(() => new JwtService(""));
        }

        [Fact]
        public void JwtServiceVerify_ShouldReadJwtToken_IfTheJwtTokenIsValid()
        {
            string key = "8E181A76C50E956FB62F3B620945D3BA75DF1E0A3A1B8C34CDD72FFB562379C4";
            var jwtService = new JwtService(key);

            string result = jwtService.CreateUserToken(5, "email");
            string jwt = jwtService.CreateUserToken(5, "email");
            var token = jwtService.ReadToken(jwt);

            string email = token.Claims.FirstOrDefault(c => c.Type == "email").Value;
            Assert.Equal(email, "email");
        }

        [Fact]
        public void JwtServiceVerify_ShouldThrowAnError_IfTheJwtTokenHasBeenTamperedWith()
        {
            // TODO, convert payload to json and modify prop and pass it to Read.
            string key = "8E181A76C50E956FB62F3B620945D3BA75DF1E0A3A1B8C34CDD72FFB562379C4";
            var jwtService = new JwtService(key);

            string result = jwtService.CreateUserToken(5, "email");
            string jwt = jwtService.CreateUserToken(5, "email");
            string newJwt = 'c' + jwt.Substring(1, jwt.Length - 1);

            Assert.ThrowsAny<Exception>(() => jwtService.ReadToken(newJwt));
        }
    }
}