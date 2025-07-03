namespace PaymentService.Infrastructure.EventTypes
{
    internal class ChargeSucceededEvent
    {
        public required string UserId { get; set; }
        public required decimal Ammount { get; set; }
    }
}
