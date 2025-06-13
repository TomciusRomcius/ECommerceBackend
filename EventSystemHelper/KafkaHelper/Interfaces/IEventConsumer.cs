using Confluent.Kafka;

namespace EventSystemHelper.Interfaces
{
    public interface IEventConsumser
    {
        public ConsumeResult<string, string> Consume(CancellationToken cancellationToken);
    }
}
