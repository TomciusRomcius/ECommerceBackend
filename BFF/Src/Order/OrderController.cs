using BFF.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Order;

[ApiController]
[Route("[controller]")]
public class OrderController(IOrderPaymentSessionService orderPaymentSessionService, ILogger<OrderController> logger)
    : ControllerBase
{
    [HttpPost("session")]
    [Authorize]
    public async Task<IActionResult> CreateOrderPaymentSession(
        [FromQuery(Name = "testcharge")] bool testCharge,
        CancellationToken cancellationToken)
    {
        string authorizationHeader = Request.Headers.Authorization.ToString();

        using HttpResponseMessage response =
            await orderPaymentSessionService.CreateOrderPaymentSessionAsync(
                testCharge,
                authorizationHeader,
                cancellationToken);

        string body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning(
                "Create order payment session failed with status {StatusCode}: {Body}",
                response.StatusCode,
                body);
        }

        return HttpResponseUtils.FromStringBody((int)response.StatusCode, body);
    }
}
