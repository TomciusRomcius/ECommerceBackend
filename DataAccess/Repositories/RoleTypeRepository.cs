using ECommerce.DataAccess.Models;
using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;

namespace ECommerce.DataAccess.Repositories
{
    public class RoleTypeRepository(IPostgresService _postgresService) : IRoleTypeRepository
    {

        public async Task CreateAsync(RoleTypeModel roleType)
        {
            if (roleType.Name.Length == 0)
            {
                throw new ArgumentException("Role type name cannot be empty!");
            }

            string query = @"
                INSERT INTO roleTypes (name) VALUES (@name);
            ";
            QueryParameter[] parameters = [new QueryParameter("name", roleType.Name)];
            await _postgresService.ExecuteScalarAsync(query, parameters);

        }

        public Task DeleteAsync(string roleTypeId)
        {
            throw new NotImplementedException();
        }

        public async Task<RoleTypeModel?> FindByIdAsync(string roleTypeId)
        {
            RoleTypeModel? result = null;

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
                    result = new RoleTypeModel(roleTypeId, name);
                }
            }

            return result;
        }

        public async Task<RoleTypeModel?> FindByNameAsync(string roleTypeName)
        {
            RoleTypeModel? result = null;

            string query = @"
                SELECT * FROM roleTypes WHERE name = @name;
            ";

            QueryParameter[] parameters = [new QueryParameter("name", roleTypeName)];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);
            if (rows.Count > 0 && rows[0].ContainsKey("roletypeid"))
            {
                string roleTypeId = rows[0]["roletypeid"].ToString()!;
                result = new RoleTypeModel(roleTypeId, roleTypeName);
            }

            return result;
        }

        public Task UpdateAsync(RoleTypeModel roleType)
        {
            throw new NotImplementedException();
        }
    }
}