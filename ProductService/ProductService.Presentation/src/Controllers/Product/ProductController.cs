using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.UseCases.Product.Commands;
using ProductService.Application.UseCases.Product.Queries;
using ProductService.Domain.Entities;
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

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromBody] RequestGetProductsDto dto)
    {
        List<ProductEntity> products = await _mediator.Send(new GetProductsQuery(dto.ProductIds ?? []));
        return Ok(products);
    }

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