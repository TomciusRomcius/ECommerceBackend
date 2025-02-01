using ECommerce.DataAccess.Utils;
using ECommerce.PaymentSession;
using Microsoft.Extensions.Logging;
using Moq;
namespace DataAccess.Test;

public class StripeSessionServiceTest
{
    readonly Mock<ILogger> _logger = new Mock<ILogger>();
    readonly IStripeSessionService _stripeSessionService;

    public StripeSessionServiceTest()
    {
        _stripeSessionService = new StripeSessionService(_logger.Object);
    }

    [Fact]
    public async Task CreateSession_ShouldReturnANewSession()
    {
        _stripeSessionService.GeneratePaymentSession("uid");
    }
}
