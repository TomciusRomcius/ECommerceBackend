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
                VALUES (@userId, @email, @passwordHash, @firstname, @lastname)
            ";


            QueryParameter[] parameters = [
                new("userId", new Guid(user.UserId)),
                new("email", user.Email),
                new("passwordHash", user.PasswordHash),
                new("firstname", user.Firstname),
                new("lastname", user.Lastname)
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public async Task DeleteAsync(string userId)
        {
            string query = @$"
                DELETE FROM users WHERE userId = @id 
            ";

            QueryParameter[] parameters = [new QueryParameter("id", userId)];
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
                string? userId = rows[0]["userid"].ToString();
                string? email = rows[0]["email"].ToString();
                string? passwordHash = rows[0]["passwordHash"].ToString();
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

        public async Task UpdateAsync(UserModel user)
        {
            string query = @"
                UPDATE USERS
                SET
                    email = COALESCE(@email, email),
                    passwordHash = COALESCE(@passwordHash, passwordHash),
                    firstname = COALESCE(@firstname, firstmane),
                    lastname = COALESCE(@lastname, lastname),
                WHERE userid = @userId
            ";

            QueryParameter[] parameters = [
                new QueryParameter("email", user.Email),
                new QueryParameter("passwordHash", user.PasswordHash),
                new QueryParameter("firstname", user.Firstname),
                new QueryParameter("lastname", user.Lastname)
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }
    }
}