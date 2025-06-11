using System.ComponentModel.DataAnnotations;

namespace ECommerce.Presentation.src.Controllers.Address.dtos;

public class DeleteAddressDto
{
    [Required] public bool IsShipping { get; set; }
}