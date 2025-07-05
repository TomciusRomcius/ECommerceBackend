using ECommerce.Domain.Models;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Repositories;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Presentation.src.Identity;

public class PostgresRoleStore(
    IRoleTypeRepository _roleTypeRepository,
    ILookupNormalizer _keyNormalizer,
    ILogger<PostgresRoleStore> _logger
)
    : IRoleStore<ApplicationUserRole>
{
    public async Task<IdentityResult> CreateAsync(ApplicationUserRole role, CancellationToken cancellationToken)
    {
        if (role.Name is null) throw new InvalidOperationException("Name is null!");

        role.NormalizedName = _keyNormalizer.NormalizeName(role.Name);

        var success = true;

        try
        {
            await _roleTypeRepository.CreateAsync(new CreateRoleTypeModel(role.NormalizedName));
        }

        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            success = false;
        }

        return success ? IdentityResult.Success : IdentityResult.Failed();
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationUserRole role, CancellationToken cancellationToken)
    {
        var success = true;

        try
        {
            await _roleTypeRepository.DeleteAsync(role.Id);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            success = false;
        }

        return success ? IdentityResult.Success : IdentityResult.Failed();
    }

    public void Dispose()
    {
    }

    public Task<ApplicationUserRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ApplicationUserRole?> FindByNameAsync(string normalizedRoleName,
        CancellationToken cancellationToken)
    {
        ApplicationUserRole? result = null;

        RoleTypeEntity? roleType = await _roleTypeRepository.FindByNameAsync(normalizedRoleName);
        if (roleType is not null)
            result = new ApplicationUserRole
            {
                Id = roleType.RoleTypeId,
                NormalizedName = normalizedRoleName
            };

        return result;
    }

    public Task<string?> GetNormalizedRoleNameAsync(ApplicationUserRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.NormalizedName);
    }

    public Task<string> GetRoleIdAsync(ApplicationUserRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Id.ToString());
    }

    public Task<string?> GetRoleNameAsync(ApplicationUserRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Name);
    }

    public Task SetNormalizedRoleNameAsync(ApplicationUserRole role, string? normalizedName,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(role.NormalizedName);
    }

    public Task SetRoleNameAsync(ApplicationUserRole role, string? roleName, CancellationToken cancellationToken)
    {
        role.Name = roleName;
        return Task.CompletedTask;
    }

    public Task<IdentityResult> UpdateAsync(ApplicationUserRole role, CancellationToken cancellationToken)
    {
        if (role.Name is null) throw new InvalidOperationException("Role name is null!");

        _roleTypeRepository.UpdateAsync(new UpdateRoleTypeModel(role.Id, role.Name));
        throw new NotImplementedException();
    }
}