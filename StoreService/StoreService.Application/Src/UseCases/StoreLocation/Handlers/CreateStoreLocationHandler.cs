using MediatR;
using Microsoft.Extensions.Logging;
using StoreService.Application.Persistence;
using StoreService.Application.UseCases.StoreLocation.Commands;
using StoreService.Domain.Entities;

namespace StoreService.Application.UseCases.StoreLocation.Handlers;

public class CreateStoreLocationHandler : IRequestHandler<CreateStoreLocationCommand, StoreLocationEntity?>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<CreateStoreLocationHandler> _logger;

    public CreateStoreLocationHandler(DatabaseContext context, ILogger<CreateStoreLocationHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<StoreLocationEntity?> Handle(CreateStoreLocationCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogTrace(
            "Handling CreateStoreLocationCommand for DisplayName: {DisplayName}, Address: {Address}",
            request.StoreLocation.DisplayName,
            request.StoreLocation.Address
        );

        var entity = new StoreLocationEntity(request.StoreLocation.DisplayName, request.StoreLocation.Address);

        _logger.LogDebug("Creating StoreLocationEntity instance: {@Entity}", entity);

        await _context.StoreLocations.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync();

        _logger.LogDebug("StoreLocationEntity persisted with StoreLocationId: {StoreLocationId}",
            entity.StoreLocationId);
        return entity;
    }
}