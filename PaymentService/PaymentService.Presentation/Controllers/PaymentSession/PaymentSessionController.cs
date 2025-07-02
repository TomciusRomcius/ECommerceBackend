using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Models;

namespace PaymentService.Presentation.Controllers.PaymentSession
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentSessionController : ControllerBase
    {
        private readonly IPaymentSessionFactory _paymentSessionFactory;

        public PaymentSessionController(IPaymentSessionFactory paymentSessionFactory)
        {
            _paymentSessionFactory = paymentSessionFactory;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Guid userId, [FromBody] int priceCents, [FromBody] PaymentProvider paymentProvider)
        {
            // TODO: error handling
            IPaymentSessionService paymentSessionService = _paymentSessionFactory.CreatePaymentSessionService(paymentProvider);
            PaymentProviderSession session = await paymentSessionService.GeneratePaymentSession(new GeneratePaymentSessionOptions
            {
                UserId = userId.ToString(),
                Price = priceCents
            });

            return Created("", session);
        }

        [HttpDelete]
        public void Delete([FromQuery] Guid userId)
        {
            // TODO: delete
        }
    }
}
