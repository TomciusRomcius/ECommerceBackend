using System.ComponentModel.DataAnnotations;

namespace ECommerce.Presentation.src.Controllers.ProductStoreLocation.dtos;

public class RemoveProductFromStoreDto
{
    [Required] public int StoreLocationId { get; set; }

    [Required] public int ProductId { get; set; }
}