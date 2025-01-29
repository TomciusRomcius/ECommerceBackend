using ECommerce.Common.Utils;
using ECommerce.DataAccess.Models;
using ECommerce.DataAccess.Repositories;
using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;
using Microsoft.Extensions.Logging;
using Moq;
namespace DataAccess.Test;

public class UserRepositoryTest
{
    Mock<IPostgresService> _postgresService = new Mock<IPostgresService>();
    Mock<ILogger> _logger = new Mock<ILogger>();
    IUserRepository _userRepository;

    public UserRepositoryTest()
    {
        _userRepository = new UserRepository(_postgresService.Object, _logger.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateANewUser()
    {
        string userId = Guid.NewGuid().ToString();
        string email = "email@gmail.com";
        string passwordHash = PasswordHasher.Hash("cool pass");
        string firstname = "kestutis";
        string lastname = "butkevicius";

        var userModel = new UserModel(userId, email, passwordHash, firstname, lastname);

        QueryParameter[] capturedParameters = [];

        _postgresService.Setup(postgres => postgres.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<QueryParameter[]>()))
        .Callback<string, QueryParameter[]>((sql, parameters) => capturedParameters = parameters).ReturnsAsync(null);

        await _userRepository.CreateAsync(userModel);

        Assert.Equal(5, capturedParameters.Length);
        Assert.Equal(new Guid(userId), capturedParameters[0].Value);
        Assert.Equal(email, capturedParameters[1].Value);
        Assert.Equal(passwordHash, capturedParameters[2].Value);
        Assert.Equal(firstname, capturedParameters[3].Value);
        Assert.Equal(lastname, capturedParameters[4].Value);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteAUser()
    {
        Guid userId = Guid.NewGuid();

        QueryParameter[] capturedParameters = [];

        _postgresService.Setup(postgres => postgres.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<QueryParameter[]>()))
        .Callback<string, QueryParameter[]>((sql, parameters) => capturedParameters = parameters).ReturnsAsync(null);

        await _userRepository.DeleteAsync(userId.ToString());

        Assert.Single(capturedParameters);
        Assert.Equal(userId, capturedParameters[0].Value);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAUser()
    {
        Guid userId = Guid.NewGuid();
        string email = "email@gmail.com";
        string? passwordHash = null;
        string firstname = "kestutis";
        string lastname = "butkevicius";

        var userModel = new UpdateUserModel(userId.ToString(), email, passwordHash, firstname, lastname);

        QueryParameter[] capturedParameters = [];

        _ = _postgresService.Setup(postgres => postgres.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<QueryParameter[]>()))
        .Callback<string, QueryParameter[]>((sql, parameters) => capturedParameters = parameters).ReturnsAsync(null);

        await _userRepository.UpdateAsync(userModel);

        Assert.Equal(5, capturedParameters.Length);
        Assert.Equal(email, capturedParameters[0].Value);
        Assert.Equal(passwordHash, capturedParameters[1].Value);
        Assert.Equal(firstname, capturedParameters[2].Value);
        Assert.Equal(lastname, capturedParameters[3].Value);
        Assert.Equal(userId, capturedParameters[4].Value);
    }
}
