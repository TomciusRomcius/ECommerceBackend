using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.src.Interfaces;
using PaymentService.Application.src.Persistence;
using PaymentService.Domain.src.Models;

namespace PaymentService.Presentation.src.Controllers.PaymentSession
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentSessionController : ControllerBase
    {
        private readonly IPaymentSessionFactory _paymentSessionFactory;
        private readonly DatabaseContext _ctx;

        public PaymentSessionController(IPaymentSessionFactory paymentSessionFactory, DatabaseContext ctx)
        {
            _paymentSessionFactory = paymentSessionFactory;
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_ctx.PaymentSessions);
        }

        //TODO: JWT auth
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // TODO: JWT auth
        [HttpPost]
        public async Task<IActionResult> CreatePaymentSession([FromBody] CreatePaymentSessionDto dto)
        {
            // TODO: error handling
            IPaymentSessionService paymentSessionService = _paymentSessionFactory.CreatePaymentSessionService(dto.PaymentProvider);
            PaymentProviderSession session = await paymentSessionService.GeneratePaymentSession(new GeneratePaymentSessionOptions
            {
                UserId = dto.UserId.ToString(),
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
