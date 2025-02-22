using System.ComponentModel.DataAnnotations;

namespace ECommerce.ProductStoreLocation
{
    public class RemoveProductFromStoreDto
    {
        [Required]
        public int StoreLocationId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}