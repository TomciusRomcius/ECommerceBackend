using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Application.Persistence;
using UserService.Application.UseCases.Cart.Commands;

namespace UserService.Application.UseCases.Cart.Handlers;

public class EraseUserCartHandler : IRequestHandler<EraseUserCartCommand>
{
    private readonly DatabaseContext _context;
    private readonly ILogger<EraseUserCartHandler> _logger;

    public EraseUserCartHandler(ILogger<EraseUserCartHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Handle(EraseUserCartCommand request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        _logger.LogDebug("Erasing user's(id: {UserId}) cart", request.UserId);
        var userId = request.UserId.ToString();
        int rowsAffected = await _context.CartProducts
            .Where(cp => cp.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);

        if (rowsAffected > 0)
        {
            _logger.LogInformation("Erasing cart items of user: {@UserId}", userId);
        }
        else
        {
            _logger.LogWarning(@"Failed to delete erase cart items of user: {@UserId}.
                                The user does not exist or does not have any cart items",
                userId);
        }
    }
}