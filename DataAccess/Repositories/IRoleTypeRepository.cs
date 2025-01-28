using ECommerce.DataAccess.Models;

namespace ECommerce.DataAccess.Repositories
{
    public interface IRoleTypeRepository
    {
        public Task CreateAsync(RoleTypeModel roleType);
        public Task UpdateAsync(RoleTypeModel roleType);
        public Task DeleteAsync(string roleTypeId);
        public Task<RoleTypeModel?> FindByIdAsync(string roleTypeId);
        public Task<RoleTypeModel?> FindByNameAsync(string roleTypeName);
    }
}