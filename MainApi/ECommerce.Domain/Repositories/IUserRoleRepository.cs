using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

public interface IUserRoleRepository
{
    public Task AddToRoleAsync(string userid, string roleName);
    public Task RemoveFromRoleAsync(string userid, string roleName);
    public Task<IList<string>> GetRolesAsync(string userId);
    public Task<bool> IsInRoleAsync(string userid, string roleName);
    public Task<IList<UserEntity>> GetUsersInRoleAsync(string roleName);
}