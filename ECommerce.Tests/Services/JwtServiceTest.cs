using ECommerce.Common.Services;
using ECommerce.Common.Utils;
using Xunit;

namespace ECommerce.Tests
{
    public class JwtServiceTest
    {
        private JwtOptions _jwtOptions = new JwtOptions(
            "app",
            "8E181A76C50E956FB62F3B620945D3BA75DF1E0A3A1B8C34CDD72FFB562379C4",
            1
        );

        [Fact]
        public void JwtService_ShouldCreateJwtTokens()
        {
            int jwtPartsCount = 3;

            var jwtService = new JwtService(_jwtOptions);
            string result = jwtService.CreateUserToken(5, "email");
            string[] jwtParts = result.Split('.');

            Assert.NotEmpty(result);
            Assert.Equal(jwtParts.Length, jwtPartsCount);
        }

        [Fact]
        public void JwtServiceConstructor_ShouldThrowAnError_IfSecretKeyIsInvalid()
        {
            JwtOptions options1 = new JwtOptions(
                "app",
                "abc",
                1
            );

            JwtOptions options2 = new JwtOptions(
                "app",
                "abc",
                1
            );
            Assert.ThrowsAny<Exception>(() => new JwtService(options1));
            Assert.ThrowsAny<Exception>(() => new JwtService(options2));
        }

        [Fact]
        public void JwtServiceVerify_ShouldReadJwtToken_IfTheJwtTokenIsValid()
        {
            var jwtService = new JwtService(_jwtOptions);

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
            var jwtService = new JwtService(_jwtOptions);

            string result = jwtService.CreateUserToken(5, "email");
            string jwt = jwtService.CreateUserToken(5, "email");
            string newJwt = 'c' + jwt.Substring(1, jwt.Length - 1);

            Assert.ThrowsAny<Exception>(() => jwtService.ReadToken(newJwt));
        }
    }
}