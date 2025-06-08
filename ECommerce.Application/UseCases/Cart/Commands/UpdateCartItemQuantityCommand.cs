using ECommerce.Domain.Entities;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Commands;

public record UpdateCartItemQuantityCommand(CartProductEntity CartProduct) : IRequest<ResultError?>;