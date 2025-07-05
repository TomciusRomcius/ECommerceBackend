using MediatR;

namespace ECommerce.Application.src.UseCases.Store.Commands;

public record RemoveProductFromStoreCommand(int StoreLocationId, int ProductId) : IRequest;