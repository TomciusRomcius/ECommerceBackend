using ECommerce.Application.src.UseCases.Cart.Commands;
using ECommerce.Domain.src.Utils;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.Cart.Handlers;

public class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand, ResultError?>
{
    private readonly ILogger<AddItemToCartHandler> _logger;
    private readonly DatabaseContext _context;

    public AddItemToCartHandler(ILogger<AddItemToCartHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<ResultError?> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        _logger.LogDebug(
            "Adding product {ProductId} to user's(id: {UserId}) cart"
            , request.CartProduct.ProductId,
            request.CartProduct.UserId
        );

        await _context.AddAsync(request.CartProduct, cancellationToken);

        try
        {
            await _context.SaveChangesAsync();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception was thrown while persisting cart item to the database.");
            return new ResultError(ResultErrorType.UNKNOWN_ERROR, "Failed to create manufacturer");
        }

        _logger.LogInformation(
            "Succesfully added product {ProductId} to user's(id: {UserId}) cart",
            request.CartProduct.ProductId,
            request.CartProduct.UserId
        );
        return null;
    }
}