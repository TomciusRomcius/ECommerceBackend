using ECommerce.Application.src.UseCases.ShippingAddress.Commands;
using ECommerce.Domain.src.Models;
using ECommerce.Persistence.src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.src.UseCases.ShippingAddress.Handlers;

public class UpdateShippingAddressHandler : IRequestHandler<UpdateShippingAddressCommand>
{
    private readonly ILogger<UpdateShippingAddressCommand> _logger;
    private readonly DatabaseContext _context;

    public UpdateShippingAddressHandler(ILogger<UpdateShippingAddressCommand> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Handle(UpdateShippingAddressCommand request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Entered Handle");
        UpdateShippingAddressModel updator = request.UpdateAddress;
        _logger.LogDebug("Shipping address updator: {@Updator}", updator);

        int rowsUpdated = await _context.ShippingAddresses
            .Where(sa => sa.ShippingAddressId == request.UpdateAddress.ShippingAddressId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(sa => sa.RecipientName, updator.RecipientName)
                .SetProperty(sa => sa.Country, updator.Country)
                .SetProperty(sa => sa.State, updator.State)
                .SetProperty(sa => sa.City, updator.City)
                .SetProperty(sa => sa.StreetAddress, updator.StreetAddress)
                .SetProperty(sa => sa.PostalCode, updator.PostalCode)
                .SetProperty(sa => sa.ApartmentUnit, updator.ApartmentUnit),
            cancellationToken);

        if (rowsUpdated > 0)
        {
            _logger.LogInformation(
                "Succesfully updated the shipping address of user: {@UserId}",
                request.UpdateAddress.UserId
            );
        }
        else
        {
            _logger.LogWarning(@"Failed to update the shipping address of user: {@UserId}
                                The user does not exist or does not have an assosiated
                                shipping address",
                                updator.UserId
            );
        }
    }
}