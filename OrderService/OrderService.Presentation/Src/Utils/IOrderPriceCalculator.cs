namespace OrderService.Utils;

public interface IOrderPriceCalculator
{
    decimal CalculatePrice(IEnumerable<CartProductModel> products);
}