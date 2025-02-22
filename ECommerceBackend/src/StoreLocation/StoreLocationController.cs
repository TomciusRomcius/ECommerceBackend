using ECommerce.Domain.Entities.StoreLocation;
using ECommerce.Domain.Models.StoreLocation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.StoreLocation
{
    [ApiController]
    [Route("[controller]")]
    public class StoreLocation : ControllerBase
    {
        readonly IStoreLocationService _storeLocationService;

        public StoreLocation(IStoreLocationService storeLocationService)
        {
            _storeLocationService = storeLocationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            List<StoreLocationEntity> result = await _storeLocationService.GetLocations();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> CreateStoreLocation([FromBody] RequestCreateLocationDto createLocationDto)
        {
            StoreLocationEntity? result = await _storeLocationService.CreateStoreLocation(
                new CreateStoreLocationModel(createLocationDto.DisplayName, createLocationDto.Address)
            );

            return Created(nameof(CreateStoreLocation), result);
        }

        [HttpPatch]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> ModifyStoreLocation([FromBody] RequestModifyLocationDto modifyLocationDto)
        {
            await _storeLocationService.UpdateStoreLocation(
                new UpdateStoreLocationModel(modifyLocationDto.StoreLocationId, modifyLocationDto.DisplayName, modifyLocationDto.Address)
            );

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> RemoveLocation([FromBody] RequestRemoveLocationDto removeLocationDto)
        {
            await _storeLocationService.RemoveLocation(removeLocationDto.StoreLocationId);
            return Ok();
        }
    }
}