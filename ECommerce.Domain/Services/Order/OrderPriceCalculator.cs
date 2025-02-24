using ECommerce.Domain.Models.CartProduct;

namespace ECommerce.Domain.Services
{
    public class OrderPriceCalculator : IOrderPriceCalculator
    {
        public decimal CalculatePrice(IEnumerable<CartProductModel> products)
        {
            decimal result = 0m;

            foreach (var product in products)
            {
                result += product.Price;
            }

            return result;
        }
    }
}