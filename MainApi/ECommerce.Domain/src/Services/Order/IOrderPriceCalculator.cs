using ECommerce.Domain.src.Models;

namespace ECommerce.Domain.src.Services.Order;

public interface IOrderPriceCalculator
{
    decimal CalculatePrice(IEnumerable<CartProductModel> products);
}