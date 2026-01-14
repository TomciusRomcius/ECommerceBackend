using OrderService.Utils;

namespace OrderService.InitializeOrder;

public interface IUserCartService
{
    Task<Result<IEnumerable<CartProductModel>>> GetUserCartProductModelsAsync(Guid userId);
}
