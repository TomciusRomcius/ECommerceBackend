using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Application.Persistence;
using UserService.Application.UseCases.Cart.Commands;
using UserService.Domain.Utils;

namespace UserService.Application.UseCases.Cart.Handlers;

public class UpdateCartItemQuantityHandler : IRequestHandler<UpdateCartItemQuantityCommand, ResultError?>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<UpdateCartItemQuantityHandler> _logger;

    public UpdateCartItemQuantityHandler(ILogger<UpdateCartItemQuantityHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<ResultError?> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        _logger.LogDebug(
            "Updating cart product: {ProductId} quantity for user: {UserId} with new quantity: {Quantity}",
            request.CartProduct.ProductId,
            request.CartProduct.UserId,
            request.CartProduct.Quantity
        );
        int rowsAffected = await _context.CartProducts
            .Where(cp => cp.UserId == request.CartProduct.UserId && cp.ProductId == request.CartProduct.ProductId)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(cp => cp.Quantity, request.CartProduct.Quantity), cancellationToken);

        if (rowsAffected > 0)
        {
            _logger.LogInformation(
                "Updated cart product: {ProductId} quantity for user: {UserId} with new quantity: {Quantity}",
                request.CartProduct.ProductId,
                request.CartProduct.UserId,
                request.CartProduct.Quantity
            );
        }
        else
        {
            _logger.LogWarning(
                @"Trying to update cart product: {ProductId} quantity for user: {UserId} 
                when user or product does not exist or is not in the cart",
                request.CartProduct.ProductId,
                request.CartProduct.UserId
            );
        }
        return null;
    }
}