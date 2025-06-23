using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Tests.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using TestUtils;

namespace ECommerce.Infrastructure.Tests.Integration.Repositories;

public class UserRepositoryIntegrationTest
{
    [Fact]
    public async Task ShouldSuccesfullyCreateAndRetrieveTheUser()
    {
        var testContainer = new TestDatabase();
        IUserRepository userRepository = RepositoryFactories.CreateUserRepository(testContainer._postgresService);

        var id = new Guid();
        await userRepository.CreateAsync(new UserEntity(id.ToString(), "email@gmail.com", "passwordhash", "firstname",
            "lastname"));

        Result<UserEntity?> result = await userRepository.FindByEmailAsync("email@gmail.com");

        Assert.Empty(result.Errors);
        UserEntity? retrieved = result.GetValue();

        Assert.NotNull(retrieved);
        Assert.Equal("email@gmail.com", retrieved.Email);
        Assert.Equal(id.ToString(), retrieved.UserId);

        await testContainer.DisposeAsync();
    }
}