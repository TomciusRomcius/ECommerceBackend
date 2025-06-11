using ECommerce.Domain.Models;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using Moq;

namespace ECommerce.Infrastructure.Tests.Unit;

public class RoleTypeRepositoryTest
{
    private readonly Mock<IPostgresService> _postgresService = new();
    private readonly IRoleTypeRepository _roleTypeRepository;

    public RoleTypeRepositoryTest()
    {
        _roleTypeRepository = new RoleTypeRepository(_postgresService.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCorrectlyPassParametersToPostgres()
    {
        var name = "administrator";

        var model = new CreateRoleTypeModel(name);

        QueryParameter[] capturedParameters = [];

        _postgresService
            .Setup(postgres => postgres.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<QueryParameter[]>()))
            .Callback<string, QueryParameter[]>((sql, parameters) => capturedParameters = parameters).ReturnsAsync(1);

        await _roleTypeRepository.CreateAsync(model);

        Assert.Equal(name, capturedParameters[0].Value);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCorrectlyPassParametersToPostgres()
    {
        var id = 1;

        QueryParameter[] capturedParameters = [];

        _postgresService
            .Setup(postgres => postgres.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<QueryParameter[]>()))
            .Callback<string, QueryParameter[]>((sql, parameters) => capturedParameters = parameters).ReturnsAsync(1);

        await _roleTypeRepository.DeleteAsync(id);

        Assert.Equal(id, capturedParameters[0].Value);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCorrectlyPassParametersToPostgres()
    {
        var name = "administrator";
        var id = 5;

        var model = new UpdateRoleTypeModel(id, name);

        QueryParameter[] capturedParameters = [];

        _postgresService
            .Setup(postgres => postgres.ExecuteScalarAsync(It.IsAny<string>(), It.IsAny<QueryParameter[]>()))
            .Callback<string, QueryParameter[]>((sql, parameters) => capturedParameters = parameters).ReturnsAsync(1);

        await _roleTypeRepository.UpdateAsync(model);

        Assert.Equal(name, capturedParameters[0].Value);
        Assert.Equal(id, capturedParameters[1].Value);
    }
}