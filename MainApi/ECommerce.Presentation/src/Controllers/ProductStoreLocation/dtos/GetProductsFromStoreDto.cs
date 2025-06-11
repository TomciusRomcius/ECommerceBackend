using System.ComponentModel.DataAnnotations;

namespace ECommerce.Presentation.src.Controllers.ProductStoreLocation.dtos;

public class GetProductsFromStoreDto
{
    [Required] public int StoreLocationId { get; set; }
}