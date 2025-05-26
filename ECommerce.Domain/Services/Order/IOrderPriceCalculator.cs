using ECommerce.Domain.Models;

namespace ECommerce.Domain.Services.Order;

public interface IOrderPriceCalculator
{
    decimal CalculatePrice(IEnumerable<CartProductModel> products);
}