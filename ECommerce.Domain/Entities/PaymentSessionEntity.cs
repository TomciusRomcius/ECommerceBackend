using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

public class PaymentSessionEntity
{
    public PaymentSessionEntity(string paymentSessionId, Guid userId, string paymentSessionProvider)
    {
        PaymentSessionId = paymentSessionId;
        UserId = userId;
        PaymentSessionProvider = paymentSessionProvider;
    }

    [Required(AllowEmptyStrings = false, ErrorMessage = "PaymentSessionId must be provided!")]
    public string PaymentSessionId { get; set; }

    [Required(ErrorMessage = "UserId must be provided!")]
    public Guid UserId { get; set; }

    // TODO: get rid of magical strings
    [Required(AllowEmptyStrings = false, ErrorMessage = "PaymentSessionProvider must not be empty!")]
    public string PaymentSessionProvider { get; set; }
}