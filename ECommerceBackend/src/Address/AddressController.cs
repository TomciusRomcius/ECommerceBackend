using System.Security.Claims;
using ECommerce.Domain.Entities.ShippingAddress;
using ECommerce.Domain.Models.ShippingAddress;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Address
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return new UnauthorizedObjectResult("You must be logged in to get items!");
            }

            var result = await _addressService.GetAddresses(userId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] SetAddressDto setAddressDto)
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return new UnauthorizedObjectResult("You must be logged in to get items!");
            }

            var address = new ShippingAddressEntity
            {
                UserId = userId,
                RecipientName = setAddressDto.RecipientName,
                StreetAddress = setAddressDto.StreetAddress,
                ApartmentUnit = setAddressDto.ApartmentUnit,
                City = setAddressDto.City,
                State = setAddressDto.State,
                PostalCode = setAddressDto.PostalCode,
                Country = setAddressDto.Country,
                MobileNumber = setAddressDto.MobileNumber,
            };

            await _addressService.AddAddress(address);
            return Created(nameof(AddAddress), null);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressDto updateAddressDto)
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return new UnauthorizedObjectResult("You must be logged in to get items!");
            }

            var updateModel = new UpdateShippingAddressModel
            {
                ShippingAddressId = updateAddressDto.ShippingAddressId,
                UserId = userId,
                RecipientName = updateAddressDto.RecipientName,
                StreetAddress = updateAddressDto.StreetAddress,
                ApartmentUnit = updateAddressDto.ApartmentUnit,
                City = updateAddressDto.City,
                State = updateAddressDto.State,
                PostalCode = updateAddressDto.PostalCode,
                Country = updateAddressDto.Country,
                MobileNumber = updateAddressDto.MobileNumber
            };

            await _addressService.UpdateAddress(updateModel);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAddress([FromBody] DeleteAddressDto deleteAddressDto)
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return new UnauthorizedObjectResult("You must be logged in to get items!");
            }

            await _addressService.DeleteAddress(userId, deleteAddressDto.IsShipping);

            return Ok();
        }

    }
}