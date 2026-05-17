using ECommerceBackend.Utils.Jwt;
using ECommerceBackend.Utils.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Services;
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
    private readonly IStoreDetailsService _storeDetailsService;

    public ProductController(IMediator mediator, IStoreDetailsService storeDetailsService)
    {
        _mediator = mediator;
        _storeDetailsService = storeDetailsService;
    }

    /// <param name="pageNumber">Page number for pagination</param>
    /// <param name="pageSize">Number of products per page</param>
    [HttpGet]
    public async Task<IActionResult> GetProducts(int pageNumber = 0, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        Page<ProductEntity> page = await _mediator.Send(new GetProductsQuery(pageNumber, pageSize), cancellationToken);
        IReadOnlyDictionary<int, ProductStoreDetails> storeDetails =
            await _storeDetailsService.GetStoreDetailsByProductIdsAsync(page.Data.Select(p => p.ProductId), cancellationToken);

        Page<ProductWithStoreDto> response = new()
        {
            Data = ProductResponseMapper.ToDtoList(page.Data, storeDetails),
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
        IReadOnlyDictionary<int, ProductStoreDetails> storeDetails =
            await _storeDetailsService.GetStoreDetailsByProductIdsAsync(products.Select(p => p.ProductId), cancellationToken);

        return Ok(ProductResponseMapper.ToDtoList(products, storeDetails));
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
        {
            return ControllerUtils.ResultErrorToResponse(result.Errors.First());
        }

        return Created(nameof(CreateProducts), result.GetValue());
    }
}
