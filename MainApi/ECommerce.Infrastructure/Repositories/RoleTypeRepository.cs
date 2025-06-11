using System.Data;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;

namespace ECommerce.Infrastructure.Repositories;

public class RoleTypeRepository(IPostgresService _postgresService) : IRoleTypeRepository
{
    public async Task<RoleTypeEntity> CreateAsync(CreateRoleTypeModel roleType)
    {
        if (roleType.Name.Length == 0) throw new ArgumentException("Role type name cannot be empty!");

        var query = @"
                INSERT INTO roleTypes (name) 
                VALUES (@name) 
                RETURNING roleTypeId; 
            ";
        QueryParameter[] parameters = [new("name", roleType.Name)];
        object? dbId = await _postgresService.ExecuteScalarAsync(query, parameters);

        if (dbId is int) return new RoleTypeEntity(Convert.ToInt32(dbId), roleType.Name);

        throw new DataException("Failed to create role type");
    }

    public async Task DeleteAsync(int roleTypeId)
    {
        var query = @"
                DELETE ONLY FROM roleTypes
                WHERE roleTypeId = $1
            ";

        QueryParameter[] parameters =
        [
            new(roleTypeId)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }

    public async Task<RoleTypeEntity?> FindByIdAsync(int roleTypeId)
    {
        RoleTypeEntity? result = null;

        var query = @"
                SELECT * FROM roleTypes WHERE roleTypeId = @roleId;
            ";

        QueryParameter[] parameters = [new("roleId", roleTypeId)];
        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
        if (rows.Count > 0)
        {
            var name = rows[0]["name"]?.ToString();
            if (name is not null) result = new RoleTypeEntity(roleTypeId, name);
        }

        return result;
    }

    public async Task<RoleTypeEntity?> FindByNameAsync(string roleTypeName)
    {
        RoleTypeEntity? result = null;

        var query = @"
                SELECT * FROM roleTypes WHERE name = @name;
            ";

        QueryParameter[] parameters = [new("name", roleTypeName)];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
        if (rows.Count > 0 && rows[0].ContainsKey("roletypeid"))
        {
            var roleTypeId = Convert.ToInt32(rows[0]["roletypeid"]);
            result = new RoleTypeEntity(roleTypeId, roleTypeName);
        }

        return result;
    }

    public async Task UpdateAsync(UpdateRoleTypeModel roleType)
    {
        var query = @"
                UPDATE roleTypes
                SET
                    name = COALESCE($1, email),
                WHERE roleTypeId = $2;
            ";

        QueryParameter[] parameters =
        [
            new(roleType.Name),
            new(roleType.RoleTypeId)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }
}