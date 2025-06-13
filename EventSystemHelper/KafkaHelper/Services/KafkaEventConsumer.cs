using Confluent.Kafka;
using EventSystemHelper.Interfaces;
using EventSystemHelper.Utils;

namespace EventSystemHelper.Services
{
    public class KafkaEventConsumer : IEventConsumser
    {
        private readonly IConsumer<string, string> _consumer;

        public KafkaEventConsumer(KafkaConfiguration kafkaConfiguration, AutoOffsetReset autoOffsetReset, string groupId, string topic)
        {
            var cfg = new ConsumerConfig
            {
                BootstrapServers = kafkaConfiguration.Servers,
                GroupId = groupId,
                AutoOffsetReset = autoOffsetReset,
            };

            _consumer = new ConsumerBuilder<string, string>(cfg).Build();
            _consumer.Subscribe(topic);
        }

        public ConsumeResult<string, string> Consume(CancellationToken cancellationToken)
        {
            return _consumer.Consume(cancellationToken);
        }
    }
}
