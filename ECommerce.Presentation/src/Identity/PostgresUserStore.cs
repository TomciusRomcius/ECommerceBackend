using ECommerce.Application.UseCases.User.Commands;
using ECommerce.Application.UseCases.User.Queries;
using ECommerce.Domain.Entities.User;
using ECommerce.Domain.Models.User;
using ECommerce.Domain.Repositories.User;
using ECommerce.Domain.Repositories.UserRole;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Identity
{
    public class PostgresUserStore :
        IUserEmailStore<ApplicationUser>,
        IUserPasswordStore<ApplicationUser>,
        IUserRoleStore<ApplicationUser>
    {
        readonly IMediator _mediator;
        readonly IUserRoleRepository _userRoleRepository;
        readonly ILogger _logger;

        public PostgresUserStore(IUserRoleRepository userRoleRepository, ILogger logger, IMediator mediator)
        {
            _userRoleRepository = userRoleRepository;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user.NormalizedEmail is null || user.PasswordHash is null)
            {
                return IdentityResult.Failed();
            }

            try
            {
                await _mediator.Send(new CreateUserCommand(
                    new UserEntity(user.Id, user.NormalizedEmail, user.PasswordHash, user.Firstname, user.Lastname)
                ));
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return IdentityResult.Failed();
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            bool success = true;

            try
            {
                await _mediator.Send(new DeleteUserCommand(
                    new Guid(user.Id)
                ));
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                success = false;
            }

            return success ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            UserEntity? user = await _mediator.Send(new FindUserByIdQuery(new Guid(userId)));
            ApplicationUser? result = null;

            if (user is not null)
            {
                result = new ApplicationUser(user.UserId, user.Email, user.PasswordHash);
            }

            return result;
        }

        public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            // Names will be treadted as emails
            return await this.FindByEmailAsync(normalizedUserName, cancellationToken);
        }

        public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.UserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user.NormalizedEmail is null)
            {
                throw new InvalidOperationException("Normalized email is null");
            }

            if (user.PasswordHash is null)
            {
                throw new InvalidOperationException("Password hash is null");
            }

            bool success = true;

            try
            {
                var updator = new UpdateUserModel(
                    user.Id,
                    user.NormalizedEmail,
                    user.PasswordHash,
                    user.Firstname,
                    user.Lastname
                );

                await _mediator.Send(new UpdateUserCommand(updator));
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

        public Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            // TODO: implement this
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            UserEntity? user = await _mediator.Send(new FindUserByEmailQuery(normalizedEmail));

            if (user is null)
            {
                return null;
            }

            return new ApplicationUser
            {
                UserName = normalizedEmail,
                NormalizedEmail = user!.Email!,
                PasswordHash = user.PasswordHash!,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Id = user.UserId.ToString()!
            };
        }

        public Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                (user.PasswordHash != null) && (user.PasswordHash.Length > 0)
            );
        }

        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User copse");
            await _userRoleRepository.AddToRoleAsync(user.Id, roleName);
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            await _userRoleRepository.RemoveFromRoleAsync(user.Id, roleName);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return await _userRoleRepository.GetRolesAsync(user.Id);
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            return await _userRoleRepository.IsInRoleAsync(user.Id, roleName);
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var dbResult = await _userRoleRepository.GetUsersInRoleAsync(roleName);
            List<ApplicationUser> users = new List<ApplicationUser>();

            foreach (var user in dbResult)
            {
                users.Add(new ApplicationUser(user.UserId, user.Email, user.PasswordHash));
            }

            return users;
        }
    }
}