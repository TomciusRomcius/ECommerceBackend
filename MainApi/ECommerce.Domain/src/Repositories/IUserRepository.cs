using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Models;
using ECommerce.Domain.src.Utils;

namespace ECommerce.Domain.src.Repositories;

public interface IUserRepository
{
    public Task<ResultError?> CreateAsync(UserEntity user);
    public Task<ResultError?> UpdateAsync(UpdateUserModel user);
    public Task<ResultError?> DeleteAsync(string userId);
    public Task<Result<UserEntity?>> FindByIdAsync(string userId);
    public Task<Result<UserEntity?>> FindByEmailAsync(string normalizedEmail);
}