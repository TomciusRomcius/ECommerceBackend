using MediatR;

namespace ECommerce.Application.UseCases.Store.Commands
{
    public record RemoveProductFromStoreCommand(int StoreLocationId, int ProductId) : IRequest;
}