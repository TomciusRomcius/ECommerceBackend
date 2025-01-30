using System.ComponentModel.DataAnnotations;

namespace ECommerce.Cart
{
    public class UpdateItemQuantityDto
    {
        [Required]
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
