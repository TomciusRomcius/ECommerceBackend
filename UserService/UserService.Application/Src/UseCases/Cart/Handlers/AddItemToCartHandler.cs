using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Persistence;
using UserService.Application.UseCases.Cart.Commands;
using UserService.Domain.Utils;

namespace UserService.Application.UseCases.Cart.Handlers;

public class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand, ResultError?>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<AddItemToCartHandler> _logger;

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