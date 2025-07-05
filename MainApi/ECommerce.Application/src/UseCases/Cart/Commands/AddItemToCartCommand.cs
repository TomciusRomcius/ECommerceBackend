using ECommerce.Domain.src.Entities;
using ECommerce.Domain.src.Utils;
using MediatR;

namespace ECommerce.Application.src.UseCases.Cart.Commands;

public record AddItemToCartCommand(CartProductEntity cartProduct) : IRequest<ResultError?>;