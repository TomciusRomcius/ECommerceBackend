using System.ComponentModel.DataAnnotations;

namespace ECommerce.Manufacturers
{
    public class RequestCreateManufacturerDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a name")]
        public required string Name { get; set; }
    }

    public class ResponseCreateManufacturerDto
    {
        public required int ManufacturerId { get; set; }
    }
}