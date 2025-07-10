using ECommerce.Application.src.UseCases.Cart.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Utils;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.Cart.Handlers;

public class GetUserCartItemsHandler : IRequestHandler<GetUserCartItemsQuery, Result<List<CartProductEntity>>>
{
    private readonly ILogger<GetUserCartItemsHandler> _logger;
    private readonly DatabaseContext _context;

    public GetUserCartItemsHandler(ILogger<GetUserCartItemsHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<CartProductEntity>>> Handle(GetUserCartItemsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handler");
        _logger.LogDebug("Getting cart items for user: {Userid}", request.UserId);

        List<CartProductEntity> result = await _context.CartProducts
            .Where(cp => cp.UserId == request.UserId.ToString())
            .ToListAsync(cancellationToken: cancellationToken);

        _logger.LogDebug("Retrieved cart items: {@CartProducts}", result);
        return new Result<List<CartProductEntity>>(result);
    }
}