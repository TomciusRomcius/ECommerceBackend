using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Models;

namespace ECommerce.Domain.src.Repositories;

public interface IRoleTypeRepository
{
    public Task<RoleTypeEntity> CreateAsync(CreateRoleTypeModel roleType);
    public Task UpdateAsync(UpdateRoleTypeModel roleType);
    public Task DeleteAsync(int roleTypeId);
    public Task<RoleTypeEntity?> FindByIdAsync(int roleTypeId);
    public Task<RoleTypeEntity?> FindByNameAsync(string roleTypeName);
}