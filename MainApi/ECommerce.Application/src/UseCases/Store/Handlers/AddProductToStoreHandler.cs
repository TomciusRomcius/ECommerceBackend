using ECommerce.Application.src.UseCases.Store.Commands;
using ECommerce.Domain.src.Utils;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.Store.Handlers;

public class AddProductToStoreHandler : IRequestHandler<AddProductToStoreCommand, ResultError?>
{
    private readonly ILogger<AddProductToStoreHandler> _logger;
    private readonly DatabaseContext _context;

    public AddProductToStoreHandler(ILogger<AddProductToStoreHandler> logger, DatabaseContext context)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<ResultError?> Handle(AddProductToStoreCommand request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        _logger.LogDebug(
            "Adding product with id: {ProductId} to store with id: {StoreLocationId}",
            request.ProductStoreLocation.ProductId,
            request.ProductStoreLocation.StoreLocationId
        );

        _logger.LogDebug("Product store location entity: {@ProductStoreLocation}", request.ProductStoreLocation);
        await _context.ProductStoreLocations.AddAsync(request.ProductStoreLocation, cancellationToken);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation(
                "Succesfully added product with id: {ProductId} to the store",
                request.ProductStoreLocation.ProductId
            );
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Encountered an exception while adding a product to a store");
            return new ResultError(ResultErrorType.UNKNOWN_ERROR, "Failed to add the product to the store");
        }
    }
}