using ECommerceBackend.Utils.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.UseCases.Product.Commands;
using ProductService.Application.UseCases.Product.Queries;
using ProductService.Domain.Entities;
using ProductService.Domain.Utils;
using ProductService.Presentation.Controllers.Product.Dtos;
using ProductService.Presentation.Utils;
using ECommerceBackend.Utils.Jwt;

namespace ProductService.Presentation.Controllers.Product;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <param name="pageNumber">Page number for pagination</param>
    /// <param name="pageSize">Number of products per page</param>
    [HttpGet]
    public async Task<IActionResult> GetProducts(int pageNumber = 0, int pageSize = 20)
    {
        Page<ProductEntity> page = await _mediator.Send(new GetProductsQuery(pageNumber, pageSize));
        return Ok(page);
    }

    /// <param name="ids">Product ids</param>
    [HttpGet("by-ids")]
    public async Task<IActionResult> GetProductsByIds([FromQuery] List<int> ids)
    {
        Result<List<ProductEntity>> result = await _mediator.Send(new GetProductsByIdQuery(ids));

        if (result.Errors.Any()) 
            return ControllerUtils.ResultErrorToResponse(result.Errors.First());

        return Ok(result.GetValue());
    }

    [Authorize(Roles = RoleTypes.Admin)]
    [HttpPost]
    public async Task<IActionResult> CreateProducts([FromBody] RequestCreateProductDto createProductDto)
    {
        var result = await _mediator.Send(new CreateProductCommand(
            createProductDto.Name,
            createProductDto.Description,
            createProductDto.Price,
            createProductDto.ManufacturerId,
            createProductDto.CategoryId
        ));

        if (result.Errors.Any()) 
            return ControllerUtils.ResultErrorToResponse(result.Errors.First());

        return Created(nameof(CreateProducts), result.GetValue());
    }
}