using System.ComponentModel.DataAnnotations;

namespace ECommerce.Presentation.Address.dtos;

public class DeleteAddressDto
{
    [Required] public bool IsShipping { get; set; }
}