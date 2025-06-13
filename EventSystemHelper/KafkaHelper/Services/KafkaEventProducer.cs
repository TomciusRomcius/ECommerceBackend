using Confluent.Kafka;
using EventSystemHelper.Interfaces;
using EventSystemHelper.Utils;
using System.Net;
namespace EventSystemHelper.Services
{
    public class KafkaEventProducer : IEventProducer, IDisposable
    {
        private readonly IProducer<string, string> _producer;

        public KafkaEventProducer(KafkaConfiguration kafkaConfiguration)
        {
            var cfg = new ProducerConfig
            {
                BootstrapServers = kafkaConfiguration.Servers,
                ClientId = Dns.GetHostName()
            };
            _producer = new ProducerBuilder<string, string>(cfg).Build();
        }

        public async Task ProduceEventAsync(string topic, Message<string, string> message, CancellationToken cancellationToken)
        {
            await _producer.ProduceAsync(topic, message, cancellationToken);
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}
