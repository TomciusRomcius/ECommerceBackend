using ECommerce.DataAccess.Models;

namespace ECommerce.DataAccess.Repositories
{
    public interface IUserRepository
    {
        public Task CreateAsync(UserModel user);
        public Task UpdateAsync(UpdateUserModel user);
        public Task DeleteAsync(string userId);
        public Task<UserModel?> FindByIdAsync(string userId);
        public Task<UserModel?> FindByEmailAsync(string normalizedEmail);
    }
}