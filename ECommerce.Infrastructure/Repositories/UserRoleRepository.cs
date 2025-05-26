using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly ILogger _logger;
    private readonly IPostgresService _postgresService;


    public UserRoleRepository(IPostgresService postgresService, ILogger logger)
    {
        _postgresService = postgresService;
        _logger = logger;
    }

    public async Task AddToRoleAsync(string userid, string roleName)
    {
        _logger.LogInformation("User role repository");
        var query = @" 
                INSERT INTO userRoles (userId, roleTypeId)
                VALUES ($1, (
                    SELECT roleTypeId FROM roleTypes WHERE name = $2
                ));
            ";

        QueryParameter[] parameters =
        [
            new(new Guid(userid)),
            new(roleName)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }

    public async Task<IList<string>> GetRolesAsync(string userId)
    {
        var query = @"
                SELECT * FROM userRoles
                INNER JOIN roleTypes ON userRoles.roleTypeId = roleTypes.roleTypeId
                WHERE userId = $1;
            ";

        QueryParameter[] parameters = [new(new Guid(userId))];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
        var result = new List<string>();

        foreach (Dictionary<string, object> row in rows) result.Add(row.GetColumn<string>("name"));

        return result;
    }

    public Task<IList<UserEntity>> GetUsersInRoleAsync(string roleName)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsInRoleAsync(string userid, string roleName)
    {
        var query = @"
                SELECT 1 FROM userRoles
                    WHERE userId = @userId AND roleTypeId = (SELECT roleTypeId FROM roleTypes WHERE name = @roleName);
            ";

        QueryParameter[] parameters =
        [
            new("userId", new Guid(userid)),
            new("roleName", roleName)
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