using ECommerce.Presentation.src.Controllers.Auth.dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ECommerce.Presentation.src.Controllers.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AuthController> _logger;

    public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILogger<AuthController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpPost("sign-up-with-password")]
    public async Task<IActionResult> SignUpWithPassword(SignUpWithPasswordRequestDto signUpWithPasswordRequestDto)
    {
        // Pasword hash gets set in UserManager.CreateAsync
        var user = new IdentityUser(signUpWithPasswordRequestDto.Email)
        {
            Email = signUpWithPasswordRequestDto.Email
        };

        IdentityResult? result = await _userManager.CreateAsync(user, signUpWithPasswordRequestDto.Password);

        return Ok(result.Errors);
    }

    [HttpPost("sign-in-with-password")]
    public async Task<IActionResult> SignInWithPassword(SignInWithPasswordRequestDto signInWithPasswordRequestDto)
    {
        SignInResult? res = await _signInManager.PasswordSignInAsync(
            signInWithPasswordRequestDto.Email,
            signInWithPasswordRequestDto.Password,
            true,
            false
        );
        return Ok(res.ToString());
    }
}