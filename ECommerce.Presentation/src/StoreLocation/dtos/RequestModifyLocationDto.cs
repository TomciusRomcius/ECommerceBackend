using System.ComponentModel.DataAnnotations;

namespace ECommerce.Presentation.StoreLocation.dtos;

public class RequestModifyLocationDto
{
    [Required] public int StoreLocationId { get; set; }

    public string? DisplayName { get; set; }
    public string? Address { get; set; }
}