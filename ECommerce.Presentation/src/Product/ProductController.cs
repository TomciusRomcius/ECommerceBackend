using ECommerce.Application.UseCases.Product.Commands;
using ECommerce.Application.UseCases.Product.Queries;
using ECommerce.Domain.Entities.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Product
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        readonly IMediator _mediator;

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
        public async Task<IActionResult> CreateProducts([FromBody()] RequestCreateProductDto createProductDto)
        {
            ProductEntity? res = await _mediator.Send(new CreateProductCommand(
                createProductDto.Name,
                createProductDto.Description,
                createProductDto.Price,
                createProductDto.ManufacturerId,
                createProductDto.CategoryId
            ));

            return Created(nameof(CreateProducts), res);
        }
    }
}