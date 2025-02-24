using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using ECommerce.Infrastructure.Utils.DictionaryExtensions;
using ECommerce.Domain.Entities.User;
using ECommerce.Domain.Models.User;
using ECommerce.Domain.Repositories.User;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly IPostgresService _postgresService;
        readonly ILogger _logger;

        public UserRepository(IPostgresService postgresService, ILogger logger)
        {
            _postgresService = postgresService;
            _logger = logger;
        }

        public async Task CreateAsync(UserEntity user)
        {
            string query = @"
                INSERT INTO users (userId, email, passwordHash, firstname, lastname)
                VALUES ($1, $2, $3, $4, $5)
            ";

            QueryParameter[] parameters = [
                new(new Guid(user.UserId)),
                new(user.Email),
                new(user.PasswordHash),
                new(user.Firstname),
                new(user.Lastname)
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public async Task DeleteAsync(string userId)
        {
            string query = @$"
                DELETE FROM users WHERE userId = $1 
            ";

            QueryParameter[] parameters = [new QueryParameter(new Guid(userId))];
            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public async Task<UserEntity?> FindByEmailAsync(string normalizedEmail)
        {
            string query = @"
                SELECT * FROM users WHERE email = @email 
            ";

            QueryParameter[] parameters = [new QueryParameter("email", normalizedEmail)];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);

            UserEntity? user = null;
            _logger.LogInformation(normalizedEmail);

            if (rows.Count != 0)
            {
                var row = rows[0];

                user = new UserEntity(
                    row.GetColumn<Guid>("userId").ToString(),
                    row.GetColumn<string>("email"),
                    row.GetColumn<string>("passwordhash"),
                    row.GetColumn<string>("firstname"),
                    row.GetColumn<string>("lastname")
                );
            }

            return user;
        }

        public Task<UserEntity?> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(UpdateUserModel user)
        {
            string query = @"
                UPDATE users
                SET
                    email = COALESCE($1, email),
                    passwordHash = COALESCE($2, passwordHash),
                    firstname = COALESCE($3, firstname),
                    lastname = COALESCE($4, lastname)
                WHERE userid = $5;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(user?.Email),
                new QueryParameter(user?.PasswordHash),
                new QueryParameter(user?.Firstname),
                new QueryParameter(user?.Lastname),
                new QueryParameter(new Guid(user.UserId))
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }
    }
}