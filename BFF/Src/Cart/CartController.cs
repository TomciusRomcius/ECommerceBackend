using BFF.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Cart;

[ApiController]
[Route("[controller]")]
public class CartController(ICartService cartService, ILogger<CartController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetItems(CancellationToken cancellationToken)
    {
        string authorizationHeader = Request.Headers.Authorization.ToString();

        try
        {
            IReadOnlyList<CartItemWithProductDto> items =
                await cartService.GetItemsWithProductsAsync(authorizationHeader, cancellationToken);
            return Ok(new { data = items });
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "Failed to fetch cart items.");
            return StatusCode(StatusCodes.Status502BadGateway, new { error = "Failed to fetch cart items." });
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddItem(
        [FromBody] AddCartItemRequest request,
        CancellationToken cancellationToken)
    {
        string authorizationHeader = Request.Headers.Authorization.ToString();

        using HttpResponseMessage response =
            await cartService.AddItemAsync(request, authorizationHeader, cancellationToken);

        string body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Add to cart failed with status {StatusCode}: {Body}", response.StatusCode, body);
        }

        return HttpResponseUtils.FromStringBody((int)response.StatusCode, body);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> RemoveItem(
        [FromQuery] int productId,
        [FromQuery] int storeLocationId,
        CancellationToken cancellationToken)
    {
        string authorizationHeader = Request.Headers.Authorization.ToString();

        using HttpResponseMessage response =
            await cartService.RemoveItemAsync(productId, storeLocationId, authorizationHeader, cancellationToken);

        string body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Remove from cart failed with status {StatusCode}: {Body}", response.StatusCode, body);
        }

        return HttpResponseUtils.FromStringBody((int)response.StatusCode, body);
    }
}
