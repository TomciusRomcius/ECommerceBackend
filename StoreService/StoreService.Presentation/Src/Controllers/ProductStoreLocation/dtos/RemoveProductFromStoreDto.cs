using System.ComponentModel.DataAnnotations;

namespace StoreService.Presentation.Controllers.ProductStoreLocation.dtos;

public class RemoveProductFromStoreDto
{
    [Required] public int StoreLocationId { get; set; }

    [Required] public int ProductId { get; set; }
}