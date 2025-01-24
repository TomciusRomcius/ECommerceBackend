using ECommerce.Auth;
using ECommerce.Common.Services;
using ECommerce.Common.Utils;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace ECommerce.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IPostgresService> _mockPostgresService;
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockPostgresService = new Mock<IPostgresService>();
            _mockJwtService = new Mock<IJwtService>();
            _authService = new AuthService(_mockPostgresService.Object, _mockJwtService.Object);
        }

        [Fact]
        public async Task SignUpWithPassword_ShouldReturnJwtTokenAndId_WhenUserIsCreated()
        {
            var dto = new SignUpWithPasswordRequestDto()
            {
                Email = "email@gmail.com",
                Name = "Name",
                Lastname = "Lastname",
                Password = "new-password"
            };

            int expectedId = 0;
            string expectedToken = "jwt";

            _mockPostgresService.Setup(postgres => postgres.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<QueryParameter[]>())).ReturnsAsync(expectedId);
            _mockJwtService.Setup(jwt => jwt.CreateUserToken(It.IsAny<int>(), It.IsAny<string>())).Returns(expectedToken);

            AuthResponseDto resDto = await _authService.SignUpWithPassword(dto);

            Assert.Equal(resDto.userId, expectedId);
            Assert.Equal(resDto.jwtToken, expectedToken);
        }

        [Fact]
        public async Task SignUpWithPassword_ShouldReturnThrowError_IfIdIsNull()
        {
            var dto = new SignUpWithPasswordRequestDto()
            {
                Email = "email@gmail.com",
                Name = "Name",
                Lastname = "Lastname",
                Password = "new-password"
            };

            _mockPostgresService.Setup(postgres => postgres.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<QueryParameter[]>())).ReturnsAsync(null);
            _mockJwtService.Setup(jwt => jwt.CreateUserToken(It.IsAny<int>(), It.IsAny<string>())).Returns("");

            await Assert.ThrowsAnyAsync<Exception>(() => _authService.SignUpWithPassword(dto));
        }

        [Fact]
        public async Task SignInWithPassword_ShouldReturnJwtTokenAndId_WhenSuccesful()
        {
            string password = "CoolPass";
            string hashedPassword = PasswordHasher.Hash("CoolPass");

            var dto = new SignInWithPasswordRequestDto()
            {
                Email = "email@gmail.com",
                Password = password
            };

            string expectedJwt = "jwt";
            int expectedUserId = 5;

            var readList = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>()
                {
                    ["userid"] = expectedUserId,
                    ["password"] = hashedPassword,
                }
            };

            _mockPostgresService.Setup(postgres => postgres.ExecuteAsync(It.IsAny<string>(), It.IsAny<QueryParameter[]?>())).ReturnsAsync(readList);
            _mockJwtService.Setup(jwt => jwt.CreateUserToken(It.IsAny<int>(), It.IsAny<string>())).Returns(expectedJwt);

            AuthResponseDto result = await _authService.SignInWithPassword(dto);

            Assert.Equal(result.jwtToken, expectedJwt);
            Assert.Equal(result.userId, expectedUserId);
        }

        [Fact]
        public async Task SignInWithPassword_ShouldReturnInvalidPassword_WhenThePasswordIsIncorrect()
        {
            string hashedRealPassword = PasswordHasher.Hash("CoolPass1");
            string givenPassword = "CoolPass";

            var dto = new SignInWithPasswordRequestDto()
            {
                Email = "email@gmail.com",
                Password = givenPassword
            };

            string expectedJwt = "jwt";
            int expectedUserId = 5;

            var readList = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>()
                {
                    ["userid"] = expectedUserId,
                    ["password"] = hashedRealPassword,
                }
            };

            _mockPostgresService.Setup(postgres => postgres.ExecuteAsync(It.IsAny<string>(), It.IsAny<QueryParameter[]>())).ReturnsAsync(readList);
            _mockJwtService.Setup(jwt => jwt.CreateUserToken(It.IsAny<int>(), It.IsAny<string>())).Returns(expectedJwt);
            await Assert.ThrowsAsync<BadHttpRequestException>(() => _authService.SignInWithPassword(dto));
        }
    }
}