using ECommerce.Application.src.UseCases.Cart.Queries;
using ECommerce.Domain.src.Models;
using ECommerce.Domain.src.Utils;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.Cart.Handlers;

public class GetUserCartItemsDetailedHandler : IRequestHandler<GetUserCartItemsDetailedQuery, Result<List<CartProductModel>>>
{
    private readonly ILogger<GetUserCartItemsDetailedHandler> _logger;
    private readonly DatabaseContext _context;

    public GetUserCartItemsDetailedHandler(ILogger<GetUserCartItemsDetailedHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<CartProductModel>>> Handle(GetUserCartItemsDetailedQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        string userId = request.UserId.ToString();
        _logger.LogDebug("Getting cart products from user: {UserId}", userId);

        IQueryable<CartProductModel> query = from cp in _context.CartProducts
                                             join p in _context.Products on cp.ProductId equals p.ProductId
                                             where cp.UserId == userId
                                             select new CartProductModel(
                                                     userId,
                                                     cp.ProductId,
                                                     cp.StoreLocationId,
                                                     cp.Quantity,
                                                     p.Price
                                                 );
        var result = await query.ToListAsync();
        _logger.LogDebug("Retrieved cart products: {@CartProducts}", result);
        return new Result<List<CartProductModel>>(result);
    }
}