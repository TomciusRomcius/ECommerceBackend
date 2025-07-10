using ECommerce.Application.src.UseCases.StoreLocation.Commands;
using ECommerce.Domain.src.Models;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.StoreLocation.Handlers;

public class UpdateStoreLocationHandler : IRequestHandler<UpdateStoreLocationCommand>
{
    private readonly ILogger<UpdateStoreLocationHandler> _logger;
    private readonly DatabaseContext _context;

    public UpdateStoreLocationHandler(ILogger<UpdateStoreLocationHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Handle(UpdateStoreLocationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        _logger.LogDebug("Updating store location with updator: {@Updator}", request.Updator);
        UpdateStoreLocationModel updator = request.Updator;

        int rowsAffected = await _context.StoreLocations
            .Where(sl => sl.StoreLocationId == request.Updator.StoreLocationId)
            .ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(sl => sl.Address, updator.Address)
                    .SetProperty(sl => sl.DisplayName, updator.DisplayName)
            );

        if (rowsAffected > 0)
        {
            _logger.LogInformation("Succesfully updated store location");
        }
        else
        {
            _logger.LogWarning(
                "Failed to update store location: location with id: {Id} does not exist",
                updator.StoreLocationId
            );
        }
    }
}