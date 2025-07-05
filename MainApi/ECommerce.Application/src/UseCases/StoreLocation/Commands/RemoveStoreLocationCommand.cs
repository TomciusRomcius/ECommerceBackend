using MediatR;

namespace ECommerce.Application.src.UseCases.StoreLocation.Commands;

public record RemoveStoreLocationCommand(int StoreLocationId) : IRequest;