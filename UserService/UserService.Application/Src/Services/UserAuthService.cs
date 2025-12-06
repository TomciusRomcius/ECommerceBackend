using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceBackend.Utils.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using UserService.Domain.Utils;

namespace UserService.Application.Services;

public class UserAuthService : IUserAuthService
{
    private readonly bool _lockOutOnFailure = false;
    private readonly ILogger<UserAuthService> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    private readonly SymmetricSecurityKey _jwtSigningKey;
    private readonly string _jwtIssuer = "ecommerce-backend";

    public UserAuthService(ILogger<UserAuthService> logger,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager,
        JwtAuthConfiguration  jwtAuthConfiguration)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;

        // TODO: get signing key during app startup
        string? jwtSigningKey = jwtAuthConfiguration.SigningKey;
        ArgumentNullException.ThrowIfNull(jwtSigningKey);
        _jwtSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey));
    }

    public async Task<Result<string>> SignUp(string email, string password)
    {
        _logger.LogTrace("Entered {FunctionName}", nameof(SignUp));
        _logger.LogDebug("Creating user with email: {Email}", email);
        // Password hash gets set in UserManager.CreateAsync
        var user = new IdentityUser(email)
        {
            Email = email
        };

        string jwtToken = GenerateJwtToken(user);
        return new Result<string>(jwtToken);
    }

    /// <returns>JWT token for user</returns>
    public async Task<Result<string>> PasswordSignIn(string email, string password)
    {
        _logger.LogTrace("Entered {FunctionName}", nameof(PasswordSignIn));
        IdentityUser? user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            _logger.LogError("Failed sign in for user '{Email}': user does not exist!", email);

            return new Result<string>([
                new ResultError(ResultErrorType.VALIDATION_ERROR, "Incorrect email or password!")
            ]);
        }
        SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, _lockOutOnFailure);
        if (!signInResult.Succeeded)
        {
            _logger.LogError("Failed sign in for user '{Email}': incorrect password!", email);

            return new Result<string>([
                new ResultError(ResultErrorType.VALIDATION_ERROR, "Incorrect email or password!")
            ]);
        }

        string jwtToken = GenerateJwtToken(user);
        _logger.LogInformation("Successfully logged in user '{Email}'", email);
        return new Result<string>(jwtToken);
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        _logger.LogTrace("Entered {FunctionName}", nameof(GenerateJwtToken));

        ArgumentNullException.ThrowIfNull(user.Email);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "User")
        };

        var signingCredentials = new SigningCredentials(_jwtSigningKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtIssuer,
            claims: claims,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: signingCredentials
        );

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }
}