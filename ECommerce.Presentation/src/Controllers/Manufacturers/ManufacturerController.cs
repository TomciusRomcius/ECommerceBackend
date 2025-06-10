using ECommerce.Application.UseCases.Manufacturer.Commands;
using ECommerce.Application.UseCases.Manufacturer.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Utils;
using ECommerce.Presentation.src.Controllers.Manufacturers.dtos;
using ECommerce.Presentation.src.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.src.Controllers.Manufacturers;

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
    [Authorize]
    public async Task<IActionResult> GetAllManufacturers()
    {
        List<ManufacturerEntity> manufacturers = await _mediator.Send(new GetAllManufacturersQuery());
        return Ok(manufacturers);
    }

    [HttpPost]
    [Authorize(Roles = "ADMINISTRATOR")]
    public async Task<IActionResult> CreateManufacturer([FromBody] RequestCreateManufacturerDto createProductsDto)
    {
        Result<int> manufacturerResult = await _mediator.Send(new CreateManufacturerCommand(
            createProductsDto.Name
        ));

        if (manufacturerResult.Errors.Any())
        {
            ControllerUtils.ResultErrorsToResponse(manufacturerResult.Errors);
        }

        return Created(nameof(CreateManufacturer), manufacturerResult.ReturnResult);
    }
}