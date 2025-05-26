using System.ComponentModel.DataAnnotations;

namespace ECommerce.Presentation.StoreLocation.dtos;

public class RequestRemoveLocationDto
{
    [Required] public int StoreLocationId { get; set; }
}