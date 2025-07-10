using ECommerce.Application.src.UseCases.ShippingAddress.Queries;
using ECommerce.Domain.src.Entities;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.ShippingAddress.Handlers;

public class GetShippingAddressesHandler : IRequestHandler<GetShippingAddressesQuery, List<ShippingAddressEntity>>
{
    private readonly ILogger<GetShippingAddressesHandler> _logger;
    private readonly DatabaseContext _context;

    public GetShippingAddressesHandler(ILogger<GetShippingAddressesHandler> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<ShippingAddressEntity>> Handle(GetShippingAddressesQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        string userId = request.UserId.ToString();
        _logger.LogDebug("Getting shipping address for user: {UserId}", userId);

        List<ShippingAddressEntity> result = await _context.ShippingAddresses
            .Where(sa => sa.UserId == userId)
            .ToListAsync(cancellationToken: cancellationToken);

        _logger.LogDebug("Retrieved addresses: {@Addresses}", result);
        return result;
    }
}