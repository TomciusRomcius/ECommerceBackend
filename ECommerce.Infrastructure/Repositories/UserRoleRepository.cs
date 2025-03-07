using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using ECommerce.Infrastructure.Utils.DictionaryExtensions;
using ECommerce.Domain.Entities.User;
using ECommerce.Domain.Repositories.UserRole;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        readonly IPostgresService _postgresService;
        readonly ILogger _logger;


        public UserRoleRepository(IPostgresService postgresService, ILogger logger)
        {
            _postgresService = postgresService;
            _logger = logger;
        }

        public async Task AddToRoleAsync(string userid, string roleName)
        {
            _logger.LogInformation("User role repository");
            string query = @" 
                INSERT INTO userRoles (userId, roleTypeId)
                VALUES ($1, (
                    SELECT roleTypeId FROM roleTypes WHERE name = $2
                ));
            ";

            QueryParameter[] parameters = [
                new QueryParameter(new Guid(userid)),
                new QueryParameter(roleName)
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public async Task<IList<string>> GetRolesAsync(string userId)
        {
            string query = @"
                SELECT * FROM userRoles
                INNER JOIN roleTypes ON userRoles.roleTypeId = roleTypes.roleTypeId
                WHERE userId = $1;
            ";

            QueryParameter[] parameters = [new QueryParameter(new Guid(userId))];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            List<string> result = new List<string>();

            foreach (var row in rows)
            {
                result.Add(row.GetColumn<string>("name"));
            }

            return result;
        }

        public Task<IList<UserEntity>> GetUsersInRoleAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsInRoleAsync(string userid, string roleName)
        {
            string query = @"
                SELECT 1 FROM userRoles
                    WHERE userId = @userId AND roleTypeId = (SELECT roleTypeId FROM roleTypes WHERE name = @roleName);
            ";

            QueryParameter[] parameters = [
                new QueryParameter("userId", new Guid(userid)),
                new QueryParameter("roleName", roleName)
            ];

            object? result = await _postgresService.ExecuteScalarAsync(query, parameters);
            _logger.LogInformation("Is in role: {Result}", result != null);
            return result != null;
        }

        public Task RemoveFromRoleAsync(string userid, string roleName)
        {
            throw new NotImplementedException();
        }
    }
}