using ECommerce.DataAccess.Models.StoreLocation;
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
        public async Task<IActionResult> GetLocationsd()
        {
            List<StoreLocationModel> result = await _storeLocationService.GetLocations();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStoreLocation([FromBody] RequestCreateLocationDto createLocationDto)
        {
            StoreLocationModel result = await _storeLocationService.CreateStoreLocation(
                new CreateStoreLocationModel(createLocationDto.DisplayName, createLocationDto.Address)
            );

            return Created(nameof(CreateStoreLocation), result);
        }

        [HttpPatch]
        public async Task<IActionResult> ModifyStoreLocation([FromBody] RequestModifyLocationDto modifyLocationDto)
        {
            await _storeLocationService.UpdateStoreLocation(
                new UpdateStoreLocationModel(modifyLocationDto.StoreLocationId, modifyLocationDto.DisplayName, modifyLocationDto.Address)
            );

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveLocation([FromBody] RequestRemoveLocationDto removeLocationDto)
        {
            await _storeLocationService.RemoveLocation(removeLocationDto.StoreLocationId);
            return Ok();
        }
    }
}