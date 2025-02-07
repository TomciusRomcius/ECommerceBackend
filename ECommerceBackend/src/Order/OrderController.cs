using System.Security.Claims;
using ECommerce.Cart;
using ECommerce.PaymentSession;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Order
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        readonly ICartService _cartService;
        readonly IStripeSessionService _stripeSessionService;
        readonly IWebHostEnvironment _webHostEnvironment;
        readonly ILogger _logger;

        public OrderController(IStripeSessionService stripeSessionService, ICartService cartService, IWebHostEnvironment webHostEnvironment, ILogger logger)
        {
            _stripeSessionService = stripeSessionService;
            _cartService = cartService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        [HttpGet("session")]
        public async Task<IActionResult> GetOrderPaymentSession()
        {
            // Fetch session id from db
            return Ok();
        }

        [HttpPost("session")]
        public async Task<IActionResult> CreateOrderPaymentSession()
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return new UnauthorizedObjectResult("You must be logged in to create payment session!");
            }

            var items = await _cartService.GetAllUserItems(userId);

            if (items.Count == 0)
            {
                // Return invalid operation
                return BadRequest("There must be at least one cart item to be able to create a payment session");
            }

            double finalPrice = 0;
            items.ForEach(item => finalPrice += item.Price * item.Quantity);

            var session = _stripeSessionService.GeneratePaymentSession(
                new() { UserId = userId, Price = (int)(finalPrice * 100.0) }
            );

            return Created(nameof(CreateOrderPaymentSession), session);
        }

        // Creates a session and instantly confirms test payment.
        [HttpPost("session/test-confirm")]
        public async Task<IActionResult> CreateOrderPaymentSessionAndConfirm()
        {
            if (!_webHostEnvironment.IsDevelopment())
            {
                _logger.LogWarning("Trying to call order/session/test-confirm on a non-development environment");
                return NotFound();
            }

            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return new UnauthorizedObjectResult("You must be logged in to create payment session!");
            }

            var items = await _cartService.GetAllUserItems(userId);

            if (items.Count == 0)
            {
                // Return invalid operation
                return BadRequest("There must be at least one cart item to be able to create a payment session");
            }

            double finalPrice = 0;
            items.ForEach(item => finalPrice += item.Price * item.Quantity);

            if (finalPrice > 0)
            {
                _stripeSessionService.GeneratePaymentSessionAndConfirm(
                    new() { UserId = userId, Price = (int)(finalPrice * 100.0) }
                );

                return Ok();
            }

            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Price calculation error");
            }
        }
    }
}