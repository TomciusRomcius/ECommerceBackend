using MediatR;

namespace ECommerce.Application.UseCases.StoreLocation.Commands;

public record RemoveStoreLocationCommand(int StoreLocationId) : IRequest;