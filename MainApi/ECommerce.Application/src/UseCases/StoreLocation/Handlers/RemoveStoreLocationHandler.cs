using ECommerce.Application.src.UseCases.StoreLocation.Commands;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.StoreLocation.Handlers;

public class RemoveStoreLocationHandler : IRequestHandler<RemoveStoreLocationCommand>
{
    private readonly ILogger<RemoveStoreLocationHandler> _logger;
    private readonly DatabaseContext _context;

    public RemoveStoreLocationHandler(ILogger<RemoveStoreLocationHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Handle(RemoveStoreLocationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        _logger.LogInformation("Removing store location: {StoreLocationId}", request.StoreLocationId);

        int rowsDeleted = await _context.StoreLocations
            .Where(sl => sl.StoreLocationId == request.StoreLocationId)
            .ExecuteDeleteAsync();

        if (rowsDeleted == 0)
        {
            _logger.LogWarning("No store location found with id: {StoreLocationId}", request.StoreLocationId);
        }
        else
        {
            _logger.LogInformation("Deleted rows: {DeletedRows}", rowsDeleted);
        }
    }
}