using System.Security.Cryptography;
using ECommerce.Common.Services;
using ECommerce.Common.Utils;
using Npgsql;

namespace ECommerce.Auth
{
    public class AuthService : IAuthService
    {
        readonly PostgresService _postgresService;
        readonly JwtService _jwtService;
        public AuthService(PostgresService postgresService, JwtService jwtService)
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
            SHA1 sha1 = SHA1.Create();
            string passwordHash = PasswordHasher.Hash(signUpWithPasswordRequestDto.Password);

            var cmd = new NpgsqlCommand(query, _postgresService.Connection)
            {
                Parameters = {
                    new("name", signUpWithPasswordRequestDto.Name),
                    new("lastname", signUpWithPasswordRequestDto.Lastname),
                    new("email", signUpWithPasswordRequestDto.Email),
                    new("password", passwordHash),
                }
            };

            var id = await cmd.ExecuteScalarAsync();

            if (id == null || !(id is int))
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

            var cmd = new NpgsqlCommand(query, _postgresService.Connection)
            {
                Parameters = {
                    new("Email", signInWithPasswordRequestDto.Email)
                }
            };
            var reader = cmd.ExecuteReader();
            await reader.ReadAsync();
            // TODO: add salt
            int id = reader.GetInt32(0);
            string retrievedPasswordHash = reader.GetString(1);

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
