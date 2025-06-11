using System.ComponentModel.DataAnnotations;

namespace ECommerce.Presentation.src.Controllers.Cart.dtos;

public class UpdateItemQuantityDto
{
    [Required] public int ProductId { get; set; }

    public int Quantity { get; set; } = 1;
}