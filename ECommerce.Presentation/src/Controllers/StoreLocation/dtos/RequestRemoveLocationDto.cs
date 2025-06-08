using System.ComponentModel.DataAnnotations;

namespace ECommerce.Presentation.src.Controllers.StoreLocation.dtos;

public class RequestRemoveLocationDto
{
    [Required] public int StoreLocationId { get; set; }
}