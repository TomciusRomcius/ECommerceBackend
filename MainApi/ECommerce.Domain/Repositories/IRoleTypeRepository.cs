using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;

namespace ECommerce.Domain.Repositories;

public interface IRoleTypeRepository
{
    public Task<RoleTypeEntity> CreateAsync(CreateRoleTypeModel roleType);
    public Task UpdateAsync(UpdateRoleTypeModel roleType);
    public Task DeleteAsync(int roleTypeId);
    public Task<RoleTypeEntity?> FindByIdAsync(int roleTypeId);
    public Task<RoleTypeEntity?> FindByNameAsync(string roleTypeName);
}