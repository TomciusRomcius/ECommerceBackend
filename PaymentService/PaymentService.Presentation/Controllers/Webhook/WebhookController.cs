using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Presentation.Controllers.Webhook
{
    [ApiController]
    [Route("[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(ILogger<WebhookController> logger)
        {
            _logger = logger;
        }

        [HttpPost("stripe")]
        public IActionResult StripeWebhook()
        {
            return Ok();
        }
    }
}
