using ECommerceBackend.Utils.Jwt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.UseCases.Manufacturer.Commands;
using ProductService.Application.UseCases.Manufacturer.Queries;
using ProductService.Presentation.Controllers.Manufacturers.Dtos;
using ProductService.Presentation.Utils;

namespace ProductService.Presentation.Controllers.Manufacturers;

[ApiController]
[Route("[controller]")]
public class ManufacturerController : ControllerBase
{
    private readonly IMediator _mediator;

    public ManufacturerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllManufacturers()
    {
        var manufacturers = await _mediator.Send(new GetAllManufacturersQuery());
        return Ok(manufacturers);
    }

    [Authorize(Roles = RoleTypes.Admin)]
    [HttpPost]
    public async Task<IActionResult> CreateManufacturer([FromBody] RequestCreateManufacturerDto createProductsDto)
    {
        var manufacturerResult = await _mediator.Send(new CreateManufacturerCommand(
            createProductsDto.Name
        ));

        if (manufacturerResult.Errors.Any()) return ControllerUtils.ResultErrorsToResponse(manufacturerResult.Errors);

        return Created(nameof(CreateManufacturer), new
        {
            manufacturerId = manufacturerResult.GetValue()
        });
    }
}