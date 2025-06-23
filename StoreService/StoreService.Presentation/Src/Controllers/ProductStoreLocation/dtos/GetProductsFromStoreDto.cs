using System.ComponentModel.DataAnnotations;

namespace StoreService.Presentation.Controllers.ProductStoreLocation.dtos;

public class GetProductsFromStoreDto
{
    [Required] public int StoreLocationId { get; set; }
}