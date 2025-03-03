using ECommerce.Application.UseCases.Manufacturer.Commands;
using ECommerce.Application.UseCases.Manufacturer.Queries;
using ECommerce.Domain.Entities.Manufacturer;
using ECommerce.Domain.Utils;
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
            Result<ManufacturerEntity> manufacturerResult = await _mediator.Send(new CreateManufacturerCommand(
                createProductsDto.Name
            ));

            // if (manufacturerResult.Errors.Any())
            // {
            //     var firstError = manufacturerResult.Errors.First();
            //     return firstError.ErrorType switch
            //     {
            //         ResultErrorType.VALIDATION_ERROR => BadRequest(firstError.Message),
            //         ResultErrorType.INVALID_OPERATION_ERROR => BadRequest(firstError.Message),
            //         _ => StatusCode(500, new { Error = "Unexpected error" })
            //     };
            // }

            return Created(nameof(CreateManufacturer), manufacturerResult.ReturnResult);
        }
    }
}