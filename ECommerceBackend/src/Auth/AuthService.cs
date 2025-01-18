using ECommerce.Common.Services;
using ECommerce.Common.Utils;
using Npgsql;

namespace ECommerce.Auth
{
    public class AuthService : IAuthService
    {
        readonly IPostgresService _postgresService;
        readonly IJwtService _jwtService;

        public AuthService(IPostgresService postgresService, IJwtService jwtService)
        {
            _postgresService = postgresService;
            _jwtService = jwtService;
        }
        public async Task<AuthResponseDto> SignUpWithPassword(SignUpWithPasswordRequestDto signUpWithPasswordRequestDto)
        {
            string query = @"
                INSERT INTO users (name, lastname, email, password)
                VALUES (@name, @lastname, @email, @password)
                RETURNING userId;
            ";

            // TODO: add salt
            string passwordHash = PasswordHasher.Hash(signUpWithPasswordRequestDto.Password);

            QueryParameter[] parameters = [
                    new("name", signUpWithPasswordRequestDto.Name),
                    new("lastname", signUpWithPasswordRequestDto.Lastname),
                    new("email", signUpWithPasswordRequestDto.Email),
                    new("password", passwordHash)
                ];

            int? id = (int?)await _postgresService.ExecuteScalarAsync(query, parameters);

            if (id is not int)
            {
                throw new HttpRequestException("Id is null or id is not an int");
            }

            else
            {
                return new AuthResponseDto()
                {
                    jwtToken = _jwtService.CreateUserToken((int)(id), signUpWithPasswordRequestDto.Email),
                    userId = (int)id
                };
            }
        }
        public async Task<AuthResponseDto> SignInWithPassword(SignInWithPasswordRequestDto signInWithPasswordRequestDto)
        {
            string query = @"
                SELECT userId, password FROM users WHERE email = @Email
            ";

            QueryParameter[] parameters = [
                new("Email", signInWithPasswordRequestDto.Email)
            ];

            List<object> result = await _postgresService.ExecuteReaderAsync(query, parameters);

            int id = (int)result[0];
            string? retrievedPasswordHash = result[1].ToString();

            if (!PasswordHasher.Verify(signInWithPasswordRequestDto.Password, retrievedPasswordHash))
            {
                throw new BadHttpRequestException("Invalid password");
            }

            string jwt = _jwtService.CreateUserToken(
                id,
                signInWithPasswordRequestDto.Email
            );

            return new AuthResponseDto()
            {
                jwtToken = jwt,
                userId = id
            };
        }

    }
}
