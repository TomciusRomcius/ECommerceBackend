using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;

namespace ECommerce.Domain.Repositories;

public interface IUserRepository
{
    public Task CreateAsync(UserEntity user);
    public Task UpdateAsync(UpdateUserModel user);
    public Task DeleteAsync(string userId);
    public Task<UserEntity?> FindByIdAsync(string userId);
    public Task<UserEntity?> FindByEmailAsync(string normalizedEmail);
}