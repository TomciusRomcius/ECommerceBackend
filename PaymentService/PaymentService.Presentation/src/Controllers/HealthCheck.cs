using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Presentation.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheck : ControllerBase
    {
        [HttpGet()]
        public IActionResult ApiHealth()
        {
            return Ok("API working!");
        }
    }
}
