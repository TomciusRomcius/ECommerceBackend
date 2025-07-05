using ECommerce.Domain.src.Models;

namespace ECommerce.Domain.src.Services.Order;

public class OrderPriceCalculator : IOrderPriceCalculator
{
    public decimal CalculatePrice(IEnumerable<CartProductModel> products)
    {
        var result = 0m;

        foreach (CartProductModel? product in products) result += product.Price;

        return result;
    }
}