using ECommerceBackend.Utils.Jwt;
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
    public async Task<IActionResult> GetProducts(int pageNumber = 0, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        Page<ProductEntity> page = await _mediator.Send(new GetProductsQuery(pageNumber, pageSize), cancellationToken);

        Page<ProductDto> response = new()
        {
            Data = ProductResponseMapper.ToDtoList(page.Data),
            TotalCount = page.TotalCount,
            HasNextPage = page.HasNextPage,
            HasPrevPage = page.HasPrevPage,
        };

        return Ok(response);
    }

    /// <param name="ids">Product ids</param>
    [HttpGet("by-ids")]
    public async Task<IActionResult> GetProductsByIds(
        [FromQuery] List<int> ids,
        CancellationToken cancellationToken = default)
    {
        Result<List<ProductEntity>> result = await _mediator.Send(new GetProductsByIdQuery(ids), cancellationToken);

        if (result.Errors.Any())
        {
            return ControllerUtils.ResultErrorToResponse(result.Errors.First());
        }

        List<ProductEntity> products = result.GetValue();
        return Ok(ProductResponseMapper.ToDtoList(products));
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
            createProductDto.CategoryId,
            createProductDto.ImageKeys,
            createProductDto.ImageCount
        ));

        if (result.Errors.Any())
        {
            return ControllerUtils.ResultErrorToResponse(result.Errors.First());
        }

        ProductEntity product = result.GetValue();
        return Created(nameof(CreateProducts), new CreateProductResponseDto { ProductId = product.ProductId });
    }
}
