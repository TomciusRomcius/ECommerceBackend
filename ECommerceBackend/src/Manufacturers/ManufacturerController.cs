using ECommerce.DataAccess.Models.Manufacturer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Manufacturers
{
    [ApiController]
    [Route("[controller]")]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        [HttpGet()]
        [Authorize()]
        public async Task<IActionResult> GetAllManufacturers()
        {
            List<ManufacturerModel> manufacturers = await _manufacturerService.GetAllManufacturers();
            return Ok(manufacturers);
        }

        [HttpPost()]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> CreateManufacturer([FromBody()] RequestCreateManufacturerDto createProductsDto)
        {
            ManufacturerModel? model = await _manufacturerService.CreateManufacturer(createProductsDto);
            return Created(nameof(CreateManufacturer), model);
        }
    }
}