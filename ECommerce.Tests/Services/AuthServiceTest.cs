using ECommerce.Auth;
using ECommerce.Common.Services;
using Moq;
using Npgsql;
using Xunit;

namespace ECommerce.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<PostgresService> _mockPostgresService;
        private readonly Mock<JwtService> _mockJwtService;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockPostgresService = new Mock<PostgresService>();
            _mockJwtService = new Mock<JwtService>();
            _authService = new AuthService(_mockPostgresService.Object, _mockJwtService.Object);
        }

        [Fact]
        public async Task SignUpWithPassword_ShouldReturnJwtTokenAndId_WhenUserIsCreated()
        {
            string email = "email@gmail.com";
            string name = "Name";
            string lastname = "Lastname";
            string password = "new-password";

            var dto = new SignUpWithPasswordRequestDto()
            {
                Email = email,
                Name = name,
                Lastname = lastname,
                Password = password
            };

            var mockCommand = new Mock<NpgsqlCommand>();
            mockCommand.Setup(cmd => cmd.ExecuteScalarAsync()).ReturnsAsync(0);
            _mockJwtService.Setup(jwt => jwt.CreateUserToken(It.IsAny<int>(), It.IsAny<string>())).Returns("token");

            AuthResponseDto resDto = await _authService.SignUpWithPassword(dto);

            Assert.Equal(resDto.userId, 0);
            Assert.Equal(resDto.jwtToken, "token");
        }
    }
}