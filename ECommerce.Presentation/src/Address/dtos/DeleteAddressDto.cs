using System.ComponentModel.DataAnnotations;

namespace ECommerce.Address
{
    public class DeleteAddressDto
    {
        [Required]
        public bool IsShipping { get; set; }
    }
}