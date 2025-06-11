using ECommerce.Application.UseCases.Product.Commands;
using ECommerce.Application.UseCases.Product.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Utils;
using ECommerce.Presentation.src.Controllers.Product.dtos;
using ECommerce.Presentation.src.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.src.Controllers.Product;

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
    public async Task<IActionResult> GetAllProducts()
    {
        List<ProductEntity> products = await _mediator.Send(new GetAllProductsQuery());
        return Ok(products);
    }

    [HttpPost]
    [Authorize(Roles = "ADMINISTRATOR")]
    public async Task<IActionResult> CreateProducts([FromBody] RequestCreateProductDto createProductDto)
    {
        Result<ProductEntity> result = await _mediator.Send(new CreateProductCommand(
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