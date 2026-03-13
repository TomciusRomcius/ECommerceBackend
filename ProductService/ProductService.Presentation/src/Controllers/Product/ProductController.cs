using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.UseCases.Product.Commands;
using ProductService.Application.UseCases.Product.Queries;
using ProductService.Domain.Entities;
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

    /// <param name="ids">Product ids</param>
    /// <param name="pageNumber">Page number for pagination</param>
    [HttpGet]
    public async Task<IActionResult> GetProducts(List<int>? ids, int pageNumber = 0)
    {
        List<ProductEntity> products;
        if (ids != null && ids.Count != 0)
        {
            products = await _mediator.Send(new GetProductsByIdQuery(ids));
        }
        else
        {
            products = await _mediator.Send(new GetProductsQuery(pageNumber));
        }
        
        return Ok(products);
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

        if (result.Errors.Any()) return ControllerUtils.ResultErrorToResponse(result.Errors.First());

        return Created(nameof(CreateProducts), result.GetValue());
    }
}