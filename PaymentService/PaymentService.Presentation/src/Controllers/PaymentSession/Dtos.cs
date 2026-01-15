using PaymentService.Domain.src.Enums;
using System.ComponentModel.DataAnnotations;
using Stripe;

namespace PaymentService.Presentation.src.Controllers.PaymentSession
{
    public class CreatePaymentSessionDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid OrderId { get; set; }
        [Required]
        public int PriceCents { get; set; }
        [Required]
        public PaymentProvider PaymentProvider { get; set; }
    }
}
