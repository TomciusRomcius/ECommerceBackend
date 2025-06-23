using MediatR;

namespace StoreService.Application.UseCases.Store.Commands;

public record RemoveProductFromStoreCommand(int StoreLocationId, int ProductId) : IRequest;