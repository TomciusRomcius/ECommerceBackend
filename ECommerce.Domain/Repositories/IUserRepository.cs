using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Utils;

namespace ECommerce.Domain.Repositories;

public interface IUserRepository
{
    public Task<ResultError?> CreateAsync(UserEntity user);
    public Task<ResultError?> UpdateAsync(UpdateUserModel user);
    public Task<ResultError?> DeleteAsync(string userId);
    public Task<Result<UserEntity?>> FindByIdAsync(string userId);
    public Task<Result<UserEntity?>> FindByEmailAsync(string normalizedEmail);
}