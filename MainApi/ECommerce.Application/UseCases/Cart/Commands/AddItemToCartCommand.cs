using ECommerce.Domain.Entities;
using ECommerce.Domain.Utils;
using MediatR;

namespace ECommerce.Application.UseCases.Cart.Commands;

public record AddItemToCartCommand(CartProductEntity cartProduct) : IRequest<ResultError?>;