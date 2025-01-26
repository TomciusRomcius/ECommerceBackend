using ECommerce.Common.Services;
using ECommerce.Common.Utils;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace ECommerce.Auth
{
    public class PostgresUserStore : IUserEmailStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>
    {
        private readonly IPostgresService _postgresService;
        private readonly ILogger _logger = LoggerManager.GetInstance().CreateLogger("PostgresUserStore");

        public PostgresUserStore(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Creating user with email: ${user.Email}");
            string query = @"
                INSERT INTO users (email, password)
                VALUES (@email, @password)
                RETURNING userId;
            ";

            if (user.Email == null || user.PasswordHash == null)
            {
                throw new DataException("User email or password is null");
            }

            QueryParameter[] parameters = [
                new("email", user.NormalizedEmail),
                new("password", user.PasswordHash)
            ];

            int? id = (int?)await _postgresService.ExecuteScalarAsync(query, parameters);

            if (id is not int)
            {
                throw new HttpRequestException("Id is null or id is not an int");
            }


            user.Id = id.ToString();
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            int id;

            if (!int.TryParse(user.Id, out id))
            {
                throw new DataException("Id is not a number");
            }

            string query = @$"
                DELETE FROM users WHERE userId = @id 
            ";

            QueryParameter[] parameters = [new QueryParameter("id", id)];
            _postgresService.ExecuteScalarAsync(query, parameters);

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Task.FromResult<ApplicationUser?>(null);
        }

        public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.UserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {

        }

        public Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            // TODO: implement this
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Finding user email: {normalizedEmail}");

            string query = @"
                SELECT * FROM users WHERE email = @email 
            ";

            QueryParameter[] parameters = [new QueryParameter("email", normalizedEmail)];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);

            ApplicationUser? user = null;

            if (rows.Count != 0)
            {
                user = new ApplicationUser
                {
                    Email = rows[0]["email"].ToString(),
                    UserName = rows[0]["email"].ToString(),
                    PasswordHash = rows[0]["password"].ToString(),
                };
            }

            return user;
        }

        public Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                (user.PasswordHash != null) && (user.PasswordHash.Length > 0)
            );
        }
    }
}