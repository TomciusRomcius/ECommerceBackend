using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.src.Interfaces;
using PaymentService.Domain.src.Models;

namespace PaymentService.Presentation.src.Controllers.PaymentSession
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

        //TODO: JWT auth
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        // TODO: JWT auth
        [HttpPost]
        public async Task<IActionResult> CreatePaymentSession([FromBody] CreatePaymentSessionDto dto)
        {
            // TODO: error handling
            IProviderPaymentSessionService paymentSessionService = _paymentSessionFactory.CreatePaymentSessionService(dto.PaymentProvider);
            PaymentProviderSession session = await paymentSessionService.GeneratePaymentSession(new GeneratePaymentSessionOptions
            {
                UserId = dto.UserId,
                Price = dto.PriceCents
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
