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

        if (response.IsSuccessStatusCode)
        {
            string body = await response.Content.ReadAsStringAsync(cancellationToken);
            return Content(body, "application/json");
        }

        string errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogWarning(
            "Create order payment session failed with status {StatusCode}: {Body}",
            response.StatusCode,
            errorBody);
        return StatusCode((int)response.StatusCode, errorBody);
    }
}
