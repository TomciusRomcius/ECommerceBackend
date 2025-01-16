using Microsoft.AspNetCore.Mvc;

namespace Routes.TestRoute
{
	[ApiController]
	[Route("[controller]")]
	public class TestRouteController : ControllerBase
	{
		[HttpPost("sign-up-with-password")]
		public object SignUpWithPassword()
		{
			return "Test";
		}
		[HttpPost("sign-in-with-password")]
		public object SignUpWithPassword()
		{
			return "Test";
		}
	}
}