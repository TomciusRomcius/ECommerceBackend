using System.ComponentModel.DataAnnotations;

namespace ECommerce.ProductStoreLocation
{
    public class GetProductsFromStoreDto
    {
        [Required]
        public int StoreLocationId { get; set; }
    }
}