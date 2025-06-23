using UserService.Domain.Utils;

namespace UserService.Application.Services;

public interface IUserAuthService
{
    public Task<Result<string>> SignUp(string email, string password);
    public Task<Result<string>> PasswordSignIn(string email, string password);
}