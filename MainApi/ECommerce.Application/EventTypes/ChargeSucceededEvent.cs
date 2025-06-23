using MediatR;

namespace ECommerce.Application.EventTypes
{
    public class ChargeSucceededEvent : INotification
    {
        public required string UserId { get; set; }
        public required decimal Ammount { get; set; }
    }
}
