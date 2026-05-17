using ECommerceBackend.Utils.Jwt;
using ECommerceBackend.Utils.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreService.Application.UseCases.Store.Commands;
using StoreService.Application.UseCases.Store.Queries;
using StoreService.Domain.Entities;
using StoreService.Presentation.Controllers.ProductStoreLocation.dtos;
using StoreService.Presentation.Utils;

namespace StoreService.Presentation.Controllers.ProductStoreLocation;

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
    public async Task<IActionResult> GetProductsFromStore(
        [FromQuery] int storeLocationId,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        Page<ProductStoreLocationDetails> result = await _mediator.Send(
            new GetProductsFromStoreQuery(storeLocationId, pageNumber, pageSize),
            cancellationToken);
        return Ok(result);
    }

    [HttpGet("by-product-ids")]
    public async Task<IActionResult> GetByProductIds([FromQuery] List<int> ids)
    {
        List<ProductStoreLocationDetails> result =
            await _mediator.Send(new GetProductStoreLocationsByProductIdsQuery(ids));
        return Ok(result);
    }

    [Authorize(Roles = RoleTypes.Admin)]
    [HttpPost]
    public async Task<IActionResult> AddProductToStore([FromBody] AddProductToStoreDto addProductToStoreDto)
    {
        var model = new ProductStoreLocationEntity(
            addProductToStoreDto.StoreLocationId,
            addProductToStoreDto.ProductId,
            addProductToStoreDto.Stock
        );

        var error = await _mediator.Send(new AddProductToStoreCommand(model));
        return error == null ? Ok() : ControllerUtils.ResultErrorToResponse(error);
    }

    [Authorize(Roles = RoleTypes.Admin)]
    [HttpDelete]
    public async Task<IActionResult> RemoveProductFromStore(
        [FromBody] RemoveProductFromStoreDto removeProductFromStoreDto)
    {
        await _mediator.Send(
            new RemoveProductFromStoreCommand(removeProductFromStoreDto.StoreLocationId,
                removeProductFromStoreDto.ProductId
            ));
        return Ok();
    }

    [Authorize(Roles = RoleTypes.Admin)]
    [HttpPatch]
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