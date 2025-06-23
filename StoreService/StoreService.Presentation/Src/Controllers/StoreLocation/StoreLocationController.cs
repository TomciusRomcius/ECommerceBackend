using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreService.Application.UseCases.StoreLocation.Commands;
using StoreService.Application.UseCases.StoreLocation.Queries;
using StoreService.Domain.Models;
using StoreService.Presentation.Controllers.StoreLocation.dtos;

namespace StoreService.Presentation.Controllers.StoreLocation;

[ApiController]
[Route("[controller]")]
public class StoreLocation : ControllerBase
{
    private readonly IMediator _mediator;

    public StoreLocation(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetLocations()
    {
        var result = await _mediator.Send(new GetAllLocationsQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStoreLocation([FromBody] RequestCreateLocationDto createLocationDto)
    {
        var result = await _mediator.Send(new CreateStoreLocationCommand(
            new CreateStoreLocationModel(createLocationDto.DisplayName, createLocationDto.Address)
        ));

        return Created(nameof(CreateStoreLocation), result);
    }

    [HttpPatch]
    public async Task<IActionResult> ModifyStoreLocation([FromBody] RequestModifyLocationDto modifyLocationDto)
    {
        await _mediator.Send(new UpdateStoreLocationCommand(
            new UpdateStoreLocationModel(modifyLocationDto.StoreLocationId, modifyLocationDto.DisplayName,
                modifyLocationDto.Address)
        ));

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveLocation([FromBody] RequestRemoveLocationDto removeLocationDto)
    {
        await _mediator.Send(new RemoveStoreLocationCommand(removeLocationDto.StoreLocationId));
        return Ok();
    }
}