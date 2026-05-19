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

        if (response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode);
        }

        string errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogWarning("Add to cart failed with status {StatusCode}: {Body}", response.StatusCode, errorBody);
        return StatusCode((int)response.StatusCode, errorBody);
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

        if (response.IsSuccessStatusCode)
        {
            return Ok();
        }

        string errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogWarning("Remove from cart failed with status {StatusCode}: {Body}", response.StatusCode, errorBody);
        return StatusCode((int)response.StatusCode, errorBody);
    }
}
