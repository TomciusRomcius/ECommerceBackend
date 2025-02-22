using System.Security.Claims;
using System.Web;
using ECommerce.Cart;
using ECommerce.DataAccess.Entities.PaymentSession;
using ECommerce.DataAccess.Repositories.PaymentSession;
using ECommerce.PaymentSession;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ECommerce.Order
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        readonly IPaymentSessionRepository _paymentSessionRepository;
        readonly ICartService _cartService;
        readonly IStripeSessionService _stripeSessionService;
        readonly IWebHostEnvironment _webHostEnvironment;
        readonly ILogger _logger;

        public OrderController(IStripeSessionService stripeSessionService, ICartService cartService, IWebHostEnvironment webHostEnvironment, ILogger logger, IPaymentSessionRepository paymentSessionRepository)
        {
            _stripeSessionService = stripeSessionService;
            _cartService = cartService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _paymentSessionRepository = paymentSessionRepository;
        }

        [HttpGet("session")]
        public async Task<IActionResult> GetOrderPaymentSession()
        {
            // Fetch session id from db
            return Ok();
        }

        [HttpPost("session")]
        public async Task<IActionResult> CreateOrderPaymentSession(
            [FromQuery(Name = "testcharge")] bool testCharge
        )
        {
            if (testCharge && !_webHostEnvironment.IsDevelopment())
            {
                _logger.LogWarning("Trying to call order/session/test-confirm on a non-development environment");
                return BadRequest();
            }

            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return new UnauthorizedObjectResult("You must be logged in to create payment session!");
            }

            if (await _paymentSessionRepository.GetPaymentSession(new Guid(userId)) is not null)
            {
                return BadRequest("Cannot create a new payment session because there is a pending one!");
            }

            var items = await _cartService.GetAllUserItemsDetailed(userId);

            if (items.Count == 0)
            {
                return BadRequest("There must be at least one cart item to be able to create a payment session!");
            }

            decimal finalPrice = 0;
            items.ForEach(item => finalPrice += item.Price * item.Quantity);

            if (finalPrice > 0)
            {
                if (!testCharge)
                {
                    _logger.LogInformation("Testing charge webhook");

                    PaymentIntent intent = _stripeSessionService.GeneratePaymentSession(
                        new() { UserId = userId, Price = (int)(finalPrice * 100.0m) }
                    );

                    await _paymentSessionRepository.CreatePaymentSessionAsync(
                        new PaymentSessionEntity(intent.Id, new Guid(userId), "stripe")
                    );
                }

                else
                {
                    _stripeSessionService.GeneratePaymentSessionAndConfirm(
                        new() { UserId = userId, Price = (int)(finalPrice * 100.0m) }
                    );
                }

                return Ok();
            }

            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Price calculation error");
            }
        }
    }
}