using ECommerce.Domain.Entities.User;
using ECommerce.Domain.Models.User;

namespace ECommerce.Domain.Repositories.User
{
    public interface IUserRepository
    {
        public Task CreateAsync(UserEntity user);
        public Task UpdateAsync(UpdateUserModel user);
        public Task DeleteAsync(string userId);
        public Task<UserEntity?> FindByIdAsync(string userId);
        public Task<UserEntity?> FindByEmailAsync(string normalizedEmail);
    }
}