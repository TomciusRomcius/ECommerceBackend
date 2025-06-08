using ECommerce.Presentation.Common.Utils;
using ECommerce.Presentation.Identity;
using ECommerce.Presentation.src.Controllers.Auth.dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ECommerce.Presentation.src.Controllers.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger _logger = LoggerManager.GetInstance().CreateLogger("AuthController");
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;

        _logger.LogInformation((userManager != null).ToString());
    }

    [HttpPost("sign-up-with-password")]
    public async Task<IActionResult> SignUpWithPassword(SignUpWithPasswordRequestDto signUpWithPasswordRequestDto)
    {
        // Pasword hash gets set in UserManager.CreateAsync
        var user = new ApplicationUser(signUpWithPasswordRequestDto.Firstname, signUpWithPasswordRequestDto.Lastname,
            signUpWithPasswordRequestDto.Email, "");
        IdentityResult? result = await _userManager.CreateAsync(user, signUpWithPasswordRequestDto.Password);

        return Ok(result.Errors);
    }

    [HttpPost("sign-in-with-password")]
    public async Task<IActionResult> SignInWithPassword(SignInWithPasswordRequestDto signInWithPasswordRequestDto)
    {
        SignInResult? res = await _signInManager.PasswordSignInAsync(signInWithPasswordRequestDto.Email,
            signInWithPasswordRequestDto.Password, true, false);
        return Ok(res.ToString());
    }
}