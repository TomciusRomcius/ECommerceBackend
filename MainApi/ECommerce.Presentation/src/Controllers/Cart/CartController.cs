using System.Security.Claims;
using ECommerce.Application.UseCases.Cart.Commands;
using ECommerce.Application.UseCases.Cart.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Models;
using ECommerce.Domain.Utils;
using ECommerce.Presentation.src.Controllers.Cart.dtos;
using ECommerce.Presentation.src.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.src.Controllers.Cart;

[ApiController]
[Route("[controller]")]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetItems()
    {
        string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return new UnauthorizedObjectResult("You must be logged in to get items!");

        Result<List<CartProductModel>> result = await _mediator.Send(new GetUserCartItemsDetailedQuery(new Guid(userId)));
        if (result.Errors.Any())
        {
            ControllerUtils.ResultErrorToResponse(result.Errors.First());
        }

        return Ok(result.GetValue());
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddItem([FromBody] RequestAddItemDto addItemDto)
    {
        string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return new UnauthorizedObjectResult("You must be logged in to add items to cart!");

        ResultError? error = await _mediator.Send(new AddItemToCartCommand(
            new CartProductEntity(userId, addItemDto.ProductId, addItemDto.StoreLocationId, addItemDto.Quantity)
        ));

        if (error != null)
        {
            return ControllerUtils.ResultErrorToResponse(error);
        }

        return Created(nameof(AddItem), null);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> UpdateItemQuantity([FromBody] RequestAddItemDto addItemDto)
    {
        string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return new UnauthorizedObjectResult("You must be logged in to add items to cart!");

        ResultError? error = await _mediator.Send(new UpdateCartItemQuantityCommand(
            new CartProductEntity(userId, addItemDto.ProductId, addItemDto.StoreLocationId, addItemDto.Quantity)
        ));

        if (error != null)
        {
            ControllerUtils.ResultErrorToResponse(error);
        }

        return Ok();
    }
}