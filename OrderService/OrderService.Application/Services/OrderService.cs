using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Persistence;
using OrderService.Application.Utils;
using OrderService.Domain.Entities;

namespace OrderService.Application.Services;

public class OrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly DatabaseContext _dbContext;
    
    public OrderService(ILogger<OrderService> logger, DatabaseContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<ResultError?> CreateOrderAsync(OrderEntity order)
    {
        // TODO: validation
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
        return null;
    }

    public async Task<OrderEntity?> GetOrderAsync(Guid userId, Guid orderId)
    {
        OrderEntity? order = await _dbContext.Orders
            .Where(o => o.UserId == userId && o.OrderEntityId == orderId)
            .FirstOrDefaultAsync();

        return order;
    }
}
