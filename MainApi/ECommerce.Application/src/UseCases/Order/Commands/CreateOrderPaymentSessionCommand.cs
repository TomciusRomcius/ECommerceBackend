using ECommerce.Domain.src.Entities;
using MediatR;

namespace ECommerce.Application.src.UseCases.Order.Commands;

public record CreateOrderPaymentSessionCommand(
    Guid UserId,
    List<CartProductEntity> CartItems,
    Dictionary<int, int> ProductStock) : IRequest<PaymentSessionEntity>;