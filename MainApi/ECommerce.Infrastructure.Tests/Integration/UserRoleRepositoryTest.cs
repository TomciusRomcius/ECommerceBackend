using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Tests.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using TestUtils;

namespace ECommerce.Infrastructure.Tests.Integration;

public class UserRoleRepositoryIntegrationTest
{
    [Fact]
    public async Task ShouldSuccesfullyAddARoleToUser()
    {
        var testContainer = new TestDatabase();

        // Create user
        UserRepository userRepository = RepositoryFactories.CreateUserRepository(testContainer._postgresService);
        var user = new UserEntity(new Guid().ToString(), "email@gmail.com", "passwordhash", "John", "Doe");
        await userRepository.CreateAsync(user);

        // Create role
        var roleRepository = new RoleTypeRepository(testContainer._postgresService);
        var roleName = "administrator";
        await roleRepository.CreateAsync(new CreateRoleTypeModel(roleName));

        // Add user to role
        IUserRoleRepository userRoleRepository =
            new UserRoleRepository(testContainer._postgresService, new Mock<ILogger>().Object);

        await userRoleRepository.AddToRoleAsync(user.UserId, roleName);

        // Check if the role was succesfully added
        bool isInRole = await userRoleRepository.IsInRoleAsync(user.UserId, roleName);
        Assert.True(isInRole);

        await testContainer.DisposeAsync();
    }
}