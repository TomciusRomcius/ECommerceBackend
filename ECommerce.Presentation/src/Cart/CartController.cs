using System.Security.Claims;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities.CartProduct;
using ECommerce.Domain.Models.CartProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Cart
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetItems()
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return new UnauthorizedObjectResult("You must be logged in to get items!");
            }

            List<CartProductModel> result = await _cartService.GetAllUserItemsDetailed(userId);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddItem([FromBody] RequestAddItemDto addItemDto)
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return new UnauthorizedObjectResult("You must be logged in to add items to cart!");
            }


            await _cartService.AddItem(
                new CartProductEntity(userId, addItemDto.ProductId, addItemDto.StoreLocationId, addItemDto.Quantity)
            );

            return Created(nameof(AddItem), null);
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateItemQuantity([FromBody] RequestAddItemDto addItemDto)
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return new UnauthorizedObjectResult("You must be logged in to add items to cart!");
            }

            await _cartService.UpdateItemQuantity(
                new CartProductEntity(userId, addItemDto.ProductId, addItemDto.StoreLocationId, addItemDto.Quantity)
            );

            return Ok();
        }
    }
}