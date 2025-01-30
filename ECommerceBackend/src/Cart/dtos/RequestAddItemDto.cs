using System.ComponentModel.DataAnnotations;

namespace ECommerce.Cart
{
    public class RequestAddItemDto
    {
        [Required]
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
