using ECommerce.DataAccess.Models.RoleType;

namespace ECommerce.DataAccess.Repositories
{
    public interface IRoleTypeRepository
    {
        public Task<RoleTypeModel> CreateAsync(CreateRoleTypeModel roleType);
        public Task UpdateAsync(UpdateRoleTypeModel roleType);
        public Task DeleteAsync(int roleTypeId);
        public Task<RoleTypeModel?> FindByIdAsync(int roleTypeId);
        public Task<RoleTypeModel?> FindByNameAsync(string roleTypeName);
    }
}