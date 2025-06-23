using Microsoft.AspNetCore.Mvc;
using UserService.Application.Services;
using UserService.Domain.Utils;
using UserService.Presentation.Controllers.Auth.dtos;
using UserService.Presentation.Utils;

namespace UserService.Presentation.Controllers.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserAuthService _userAuthService;

    public AuthController(IUserAuthService userAuthService)
    {
        _userAuthService = userAuthService;
    }

    [HttpPost("sign-up-with-password")]
    public async Task<IActionResult> SignUpWithPassword(SignUpWithPasswordRequestDto signUpWithPasswordRequestDto)
    {
        Result<string> jwtResult = await _userAuthService.SignUp(
            signUpWithPasswordRequestDto.Email, signUpWithPasswordRequestDto.Password
        );

        if (jwtResult.Errors.Any())
        {
            return ControllerUtils.ResultErrorsToResponse(jwtResult.Errors);
        }

        return Ok(jwtResult.GetValue());
    }

    [HttpPost("sign-in-with-password")]
    public async Task<IActionResult> SignInWithPassword(SignInWithPasswordRequestDto signInWithPasswordRequestDto)
    {
        Result<string> jwtResult = await _userAuthService.PasswordSignIn(
            signInWithPasswordRequestDto.Email,
            signInWithPasswordRequestDto.Password
        );
        if (jwtResult.Errors.Any())
        {
            return ControllerUtils.ResultErrorsToResponse(jwtResult.Errors);
        }

        return Ok(jwtResult.GetValue());
    }
}