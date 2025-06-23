using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreService.Application.Persistence;
using StoreService.Application.UseCases.StoreLocation.Commands;

namespace StoreService.Application.UseCases.StoreLocation.Handlers;

public class RemoveStoreLocationHandler : IRequestHandler<RemoveStoreLocationCommand>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<RemoveStoreLocationHandler> _logger;

    public RemoveStoreLocationHandler(ILogger<RemoveStoreLocationHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Handle(RemoveStoreLocationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        _logger.LogInformation("Removing store location: {StoreLocationId}", request.StoreLocationId);

        var rowsDeleted = await _context.StoreLocations
            .Where(sl => sl.StoreLocationId == request.StoreLocationId)
            .ExecuteDeleteAsync();

        if (rowsDeleted == 0)
            _logger.LogWarning("No store location found with id: {StoreLocationId}", request.StoreLocationId);
        else
            _logger.LogInformation("Deleted rows: {DeletedRows}", rowsDeleted);
    }
}