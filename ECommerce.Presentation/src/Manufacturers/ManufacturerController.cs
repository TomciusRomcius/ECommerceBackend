using ECommerce.Application.UseCases.Manufacturer.Commands;
using ECommerce.Application.UseCases.Manufacturer.Queries;
using ECommerce.Domain.Entities.Manufacturer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Manufacturers
{
    [ApiController]
    [Route("[controller]")]
    public class ManufacturerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ManufacturerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Authorize()]
        public async Task<IActionResult> GetAllManufacturers()
        {
            List<ManufacturerEntity> manufacturers = await _mediator.Send(new GetAllManufacturersQuery());
            return Ok(manufacturers);
        }

        [HttpPost()]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> CreateManufacturer([FromBody()] RequestCreateManufacturerDto createProductsDto)
        {
            ManufacturerEntity manufacturer = await _mediator.Send(new CreateManufacturerCommand(
                createProductsDto.Name
            ));

            return Created(nameof(CreateManufacturer), manufacturer);
        }
    }
}