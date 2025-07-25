using ECommerce.Application.src.UseCases.Store.Commands;
using ECommerce.Application.src.UseCases.Store.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Utils;
using ECommerce.Presentation.src.Controllers.ProductStoreLocation.dtos;
using ECommerce.Presentation.src.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.src.Controllers.ProductStoreLocation;

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

        ResultError? error = await _mediator.Send(new AddProductToStoreCommand(model));
        return error == null ? Ok() : ControllerUtils.ResultErrorToResponse(error);
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