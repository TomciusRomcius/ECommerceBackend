using System.ComponentModel.DataAnnotations;

namespace ECommerce.Presentation.ProductStoreLocation.dtos;

public class GetProductsFromStoreDto
{
    [Required] public int StoreLocationId { get; set; }
}