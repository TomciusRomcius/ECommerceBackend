using ECommerce.Common.Utils;
using ECommerce.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger = LoggerManager.GetInstance().CreateLogger("AuthController");

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;

            _logger.LogInformation((userManager != null).ToString());
        }

        [HttpPost("sign-up-with-password")]
        public async Task<IActionResult> SignUpWithPassword(SignUpWithPasswordRequestDto signUpWithPasswordRequestDto)
        {
            var user = new ApplicationUser()
            {
                Email = signUpWithPasswordRequestDto.Email,
                UserName = signUpWithPasswordRequestDto.Email,
                Firstname = signUpWithPasswordRequestDto.Firstname,
                Lastname = signUpWithPasswordRequestDto.Lastname,
            };

            var result = await _userManager.CreateAsync(user, signUpWithPasswordRequestDto.Password);

            return Ok(result.Errors);
        }

        [HttpPost("sign-in-with-password")]
        public async Task<IActionResult> SignInWithPassword(SignInWithPasswordRequestDto signInWithPasswordRequestDto)
        {
            var res = await _signInManager.PasswordSignInAsync(signInWithPasswordRequestDto.Email, signInWithPasswordRequestDto.Password, true, false);
            return Ok(res.ToString());
        }
    }
}