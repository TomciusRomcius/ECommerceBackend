using Confluent.Kafka;

namespace EventSystemHelper.Interfaces
{
    public interface IEventProducer
    {
        Task ProduceEventAsync(string topic, Message<string, string> message, CancellationToken cancellationToken);
    }
}
