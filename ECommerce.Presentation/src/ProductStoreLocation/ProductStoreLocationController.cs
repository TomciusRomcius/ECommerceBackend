using ECommerce.Application.UseCases.Store.Commands;
using ECommerce.Application.UseCases.Store.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Presentation.ProductStoreLocation.dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.ProductStoreLocation;

[ApiController]
[Route("[controller]")]
public class ProductStoreLocationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductStoreLocationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductsFromStore([FromBody] GetProductsFromStoreDto getProductsFromStoreDto)
    {
        bool isDetailed = HttpContext.Request.Query["detailed"].FirstOrDefault() == "1";

        object result;
        if (isDetailed)
            result = await _mediator.Send(new GetProductsFromStoreQuery(getProductsFromStoreDto.StoreLocationId));

        else
            result = await _mediator.Send(new GetProductIdsFromStoreQuery(getProductsFromStoreDto.StoreLocationId));

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "ADMINISTRATOR")]
    public async Task<IActionResult> AddProductToStore([FromBody] AddProductToStoreDto addProductToStoreDto)
    {
        var model = new ProductStoreLocationEntity(
            addProductToStoreDto.StoreLocationId,
            addProductToStoreDto.ProductId,
            addProductToStoreDto.Stock
        );

        await _mediator.Send(new AddProductToStoreCommand(model));

        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "ADMINISTRATOR")]
    public async Task<IActionResult> RemoveProductFromStore(
        [FromBody] RemoveProductFromStoreDto removeProductFromStoreDto)
    {
        await _mediator.Send(
            new RemoveProductFromStoreCommand(removeProductFromStoreDto.StoreLocationId,
                removeProductFromStoreDto.ProductId
            ));
        return Ok();
    }


    [HttpPatch]
    [Authorize(Roles = "ADMINISTRATOR")]
    public async Task<IActionResult> ModifyProductFromStore([FromBody] AddProductToStoreDto addProductToStoreDto)
    {
        var model = new ProductStoreLocationEntity(
            addProductToStoreDto.StoreLocationId,
            addProductToStoreDto.ProductId,
            addProductToStoreDto.Stock
        );

        await _mediator.Send(new UpdateProductStockCommand(model));
        return Ok();
    }
}