using OrderService.Utils;

namespace OrderService.Presentation.Order;

public interface IUserCartService
{
    Task<Result<IEnumerable<CartProductModel>>> GetUserCartProductModelsAsync(Guid userId);
}
