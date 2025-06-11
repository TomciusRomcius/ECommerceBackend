using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Presentation.Controllers
{
    [ApiController]
    [Route("")]
    public class RootController : ControllerBase
    {
        private readonly ILogger<RootController> _logger;

        public RootController(ILogger<RootController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public IActionResult ApiHealth()
        {
            return Ok("API working!");
        }
    }
}
