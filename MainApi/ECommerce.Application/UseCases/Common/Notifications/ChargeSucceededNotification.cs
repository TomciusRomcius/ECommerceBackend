using MediatR;

namespace ECommerce.Application.UseCases.Common.Notifications;

public record ChargeSucceededNotification(Guid UserId) : INotification;