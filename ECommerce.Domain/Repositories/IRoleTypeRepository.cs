using ECommerce.Domain.Entities.RoleType;
using ECommerce.Domain.Models.RoleType;

namespace ECommerce.Domain.Repositories.RoleType
{
    public interface IRoleTypeRepository
    {
        public Task<RoleTypeEntity> CreateAsync(CreateRoleTypeModel roleType);
        public Task UpdateAsync(UpdateRoleTypeModel roleType);
        public Task DeleteAsync(int roleTypeId);
        public Task<RoleTypeEntity?> FindByIdAsync(int roleTypeId);
        public Task<RoleTypeEntity?> FindByNameAsync(string roleTypeName);
    }
}