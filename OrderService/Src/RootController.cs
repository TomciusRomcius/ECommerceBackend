using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.RootController;

[ApiController]
[Authorize]
[Route("/")]
public class RootController : ControllerBase
{
    [HttpGet]
    public IActionResult ApiStatus()
    {
        return Ok("API working!");
    }
}