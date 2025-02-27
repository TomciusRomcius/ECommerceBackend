using ECommerce.Application.UseCases.Queries;
using ECommerce.Application.UseCases.StoreLocation.Commands;
using ECommerce.Domain.Entities.StoreLocation;
using ECommerce.Domain.Models.StoreLocation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.StoreLocation
{
    [ApiController]
    [Route("[controller]")]
    public class StoreLocation : ControllerBase
    {
        readonly IMediator _mediator;

        public StoreLocation(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            List<StoreLocationEntity> result = await _mediator.Send(new GetAllLocationsQuery());
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> CreateStoreLocation([FromBody] RequestCreateLocationDto createLocationDto)
        {
            StoreLocationEntity? result = await _mediator.Send(new CreateStoreLocationCommand(
                new CreateStoreLocationModel(createLocationDto.DisplayName, createLocationDto.Address)
            ));

            return Created(nameof(CreateStoreLocation), result);
        }

        [HttpPatch]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> ModifyStoreLocation([FromBody] RequestModifyLocationDto modifyLocationDto)
        {
            await _mediator.Send(new UpdateStoreLocationCommand(
                new UpdateStoreLocationModel(modifyLocationDto.StoreLocationId, modifyLocationDto.DisplayName, modifyLocationDto.Address)
            ));

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> RemoveLocation([FromBody] RequestRemoveLocationDto removeLocationDto)
        {
            await _mediator.Send(new RemoveStoreLocationCommand(removeLocationDto.StoreLocationId));
            return Ok();
        }
    }
}