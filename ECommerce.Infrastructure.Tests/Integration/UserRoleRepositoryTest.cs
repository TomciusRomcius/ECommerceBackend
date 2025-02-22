using ECommerce.TestUtils.TestDatabase;
using ECommerce.DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using ECommerce.Domain.Entities.User;
using ECommerce.Domain.Models.RoleType;
using ECommerce.Domain.Repositories.UserRole;

namespace DataAccess.Tests.Integration
{
    public class UserRoleRepositoryIntegrationTest
    {
        [Fact]
        public async Task ShouldSuccesfullyAddARoleToUser()
        {
            var testContainer = new TestDatabase();

            // Create user
            UserRepository userRepository = new UserRepository(testContainer._postgresService, new Mock<ILogger>().Object);
            var user = new UserEntity(new Guid().ToString(), "email@gmail.com", "passwordhash", "John", "Doe");
            await userRepository.CreateAsync(user);

            // Create role
            var roleRepository = new RoleTypeRepository(testContainer._postgresService);
            string roleName = "administrator";
            await roleRepository.CreateAsync(new CreateRoleTypeModel(roleName));

            // Add user to role
            IUserRoleRepository userRoleRepository = new UserRoleRepository(testContainer._postgresService, new Mock<ILogger>().Object);

            await userRoleRepository.AddToRoleAsync(user.UserId, roleName);

            // Check if the role was succesfully added
            bool isInRole = await userRoleRepository.IsInRoleAsync(user.UserId, roleName);
            Assert.True(isInRole);

            await testContainer.DisposeAsync();
        }
    }
}