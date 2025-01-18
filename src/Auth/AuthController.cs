using ECommerce.Common.Services;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ECommerce.Auth
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		readonly IAuthService _authService;
		readonly ILogger _logger;
		public AuthController(IAuthService authService)
		{
			_authService = authService;
			ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
			_logger = loggerFactory.CreateLogger("AuthController");
		}
		[HttpPost("sign-up-with-password")]
		public async Task<IActionResult> SignUpWithPassword(SignUpWithPasswordRequestDto signUpWithPasswordRequestDto)
		{
			try
			{
				AuthResponseDto resDto = await _authService.SignUpWithPassword(signUpWithPasswordRequestDto);
				Response.Cookies.Append("user", resDto.jwtToken);
				return StatusCode(201, resDto);
			}
			catch (PostgresException ex)
			{
				_logger.LogInformation(ex.ToString());
				return StatusCode(500, "Encountered an error while writting to the database.");
			}

			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return StatusCode(500, "Unknown error.");
			}
		}
		[HttpPost("sign-in-with-password")]
		public async Task<IActionResult> SignInWithPassword(SignInWithPasswordRequestDto signInWithPasswordRequestDto)
		{
			AuthResponseDto res = await _authService.SignInWithPassword(signInWithPasswordRequestDto);
			return Ok(res);
		}
	}
}