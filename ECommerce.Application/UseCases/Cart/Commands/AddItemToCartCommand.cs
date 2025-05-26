using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Commands;

public record AddItemToCartCommand(CartProductEntity cartProduct) : IRequest<CartProductEntity>;