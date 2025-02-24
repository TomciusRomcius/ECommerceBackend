using System.Data;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using ECommerce.Domain.Entities.RoleType;
using ECommerce.Domain.Models.RoleType;
using ECommerce.Domain.Repositories.RoleType;

namespace ECommerce.Infrastructure.Repositories
{
    public class RoleTypeRepository(IPostgresService _postgresService) : IRoleTypeRepository
    {

        public async Task<RoleTypeEntity> CreateAsync(CreateRoleTypeModel roleType)
        {
            if (roleType.Name.Length == 0)
            {
                throw new ArgumentException("Role type name cannot be empty!");
            }

            string query = @"
                INSERT INTO roleTypes (name) 
                VALUES (@name) 
                RETURNING roleTypeId; 
            ";
            QueryParameter[] parameters = [new QueryParameter("name", roleType.Name)];
            object? dbId = await _postgresService.ExecuteScalarAsync(query, parameters);

            if (dbId is int)
            {
                return new RoleTypeEntity(Convert.ToInt32(dbId), roleType.Name);
            }

            else
            {
                throw new DataException("Failed to create role type");
            }
        }

        public async Task DeleteAsync(int roleTypeId)
        {
            string query = @"
                DELETE ONLY FROM roleTypes
                WHERE roleTypeId = $1
            ";

            QueryParameter[] parameters = [
                new QueryParameter(roleTypeId)
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }

        public async Task<RoleTypeEntity?> FindByIdAsync(int roleTypeId)
        {
            RoleTypeEntity? result = null;

            string query = @"
                SELECT * FROM roleTypes WHERE roleTypeId = @roleId;
            ";

            QueryParameter[] parameters = [new QueryParameter("roleId", roleTypeId)];
            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            if (rows.Count > 0)
            {
                string? name = rows[0]["name"]?.ToString();
                if (name is not null)
                {
                    result = new RoleTypeEntity(roleTypeId, name);
                }
            }

            return result;
        }

        public async Task<RoleTypeEntity?> FindByNameAsync(string roleTypeName)
        {
            RoleTypeEntity? result = null;

            string query = @"
                SELECT * FROM roleTypes WHERE name = @name;
            ";

            QueryParameter[] parameters = [new QueryParameter("name", roleTypeName)];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            if (rows.Count > 0 && rows[0].ContainsKey("roletypeid"))
            {

                int roleTypeId = Convert.ToInt32(rows[0]["roletypeid"]);
                result = new RoleTypeEntity(roleTypeId, roleTypeName);
            }

            return result;
        }

        public async Task UpdateAsync(UpdateRoleTypeModel roleType)
        {
            string query = @"
                UPDATE roleTypes
                SET
                    name = COALESCE($1, email),
                WHERE roleTypeId = $2;
            ";

            QueryParameter[] parameters = [
                new QueryParameter(roleType.Name),
                new QueryParameter(roleType.RoleTypeId)
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }
    }
}