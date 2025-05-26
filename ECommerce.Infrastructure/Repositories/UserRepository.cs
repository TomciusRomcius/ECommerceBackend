using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ILogger _logger;
    private readonly IPostgresService _postgresService;

    public UserRepository(IPostgresService postgresService, ILogger logger)
    {
        _postgresService = postgresService;
        _logger = logger;
    }

    public async Task CreateAsync(UserEntity user)
    {
        var query = @"
                INSERT INTO users (userId, email, passwordHash, firstname, lastname)
                VALUES ($1, $2, $3, $4, $5)
            ";

        QueryParameter[] parameters =
        [
            new(new Guid(user.UserId)),
            new(user.Email),
            new(user.PasswordHash),
            new(user.Firstname),
            new(user.Lastname)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }

    public async Task DeleteAsync(string userId)
    {
        var query = @"
                DELETE FROM users WHERE userId = $1 
            ";

        QueryParameter[] parameters = [new(new Guid(userId))];
        await _postgresService.ExecuteScalarAsync(query, parameters);
    }

    public async Task<UserEntity?> FindByEmailAsync(string normalizedEmail)
    {
        var query = @"
                SELECT * FROM users WHERE email = @email 
            ";

        QueryParameter[] parameters = [new("email", normalizedEmail)];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);

        UserEntity? user = null;
        _logger.LogInformation(normalizedEmail);

        if (rows.Count != 0)
        {
            Dictionary<string, object> row = rows[0];

            user = new UserEntity(
                row.GetColumn<Guid>("userId").ToString(),
                row.GetColumn<string>("email"),
                row.GetColumn<string>("passwordhash"),
                row.GetColumn<string>("firstname"),
                row.GetColumn<string>("lastname")
            );
        }

        return user;
    }

    public Task<UserEntity?> FindByIdAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(UpdateUserModel user)
    {
        var query = @"
                UPDATE users
                SET
                    email = COALESCE($1, email),
                    passwordHash = COALESCE($2, passwordHash),
                    firstname = COALESCE($3, firstname),
                    lastname = COALESCE($4, lastname)
                WHERE userid = $5;
            ";

        QueryParameter[] parameters =
        [
            new(user?.Email),
            new(user?.PasswordHash),
            new(user?.Firstname),
            new(user?.Lastname),
            new(new Guid(user.UserId))
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }
}