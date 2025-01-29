using ECommerce.DataAccess.Models;
using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;
using Microsoft.Extensions.Logging;

namespace ECommerce.DataAccess.Repositories
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

        public async Task CreateAsync(UserModel user)
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

        public async Task<UserModel?> FindByEmailAsync(string normalizedEmail)
        {
            string query = @"
                SELECT * FROM users WHERE email = @email 
            ";

            QueryParameter[] parameters = [new QueryParameter("email", normalizedEmail)];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);

            UserModel? user = null;
            _logger.LogInformation(normalizedEmail);

            List<string> errors = new List<string>();

            if (rows.Count != 0)
            {
                // TODO: implement a helper method for null-safety
                string? userId = rows[0]["userid"].ToString();
                string? email = rows[0]["email"].ToString();
                string? passwordHash = rows[0]["passwordhash"].ToString();
                string? firstname = rows[0]["firstname"].ToString();
                string? lastname = rows[0]["lastname"].ToString();

                if (userId is null)
                {
                    errors.Add("userId is null!");
                }

                if (email is null)
                {
                    errors.Add("email is null!");
                }

                if (passwordHash is null)
                {
                    errors.Add("passwordHash is null!");
                }

                if (firstname is null)
                {
                    errors.Add("firstname is null!");
                }

                if (lastname is null)
                {
                    errors.Add("lastname is null!");
                }

                if (errors.Count == 0)
                {
                    user = new UserModel(userId!, email!, passwordHash!, firstname!, lastname!);
                }
            }

            foreach (var error in errors)
            {
                _logger.LogError(error);
            }

            return user;
        }

        public Task<UserModel?> FindByIdAsync(string userId)
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