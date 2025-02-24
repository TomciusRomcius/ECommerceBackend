using ECommerce.Domain.Models.CartProduct;

namespace ECommerce.Domain.Services
{
    public interface IOrderPriceCalculator
    {
        decimal CalculatePrice(IEnumerable<CartProductModel> products);
    }
}