using Confluent.Kafka;
using EventSystemHelper.Services;
using EventSystemHelper.Utils;
using System.Text;

namespace EventSystemHelper.Tests.Integration
{
    public class KafkaIntegrationTest
    {
        /** Basic integration test where we setup a consumer and
         * producer and check if the consumer gets the message
         */
        [Fact]
        public async Task KafkaEventConsumerAndKafkaEventConsumer_ShouldProduceAndConsumeEvents()
        {
            var kafkaCfg = new KafkaConfiguration("localhost:9093");
            var topic = "topic";

            KafkaEventConsumer consumer = new KafkaEventConsumer(kafkaCfg, AutoOffsetReset.Earliest, "group-id", topic);
            KafkaEventProducer producer = new KafkaEventProducer(kafkaCfg);

            string value = "ABCD";

            var message = new Message<string, string>
            {
                Headers = new Headers
                {
                    { "value", Encoding.UTF8.GetBytes(value) }
                }
            };

            var pcts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await producer.ProduceEventAsync(topic, message, pcts.Token);

            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            ConsumeResult<string, string> retrieved = consumer.Consume(cts.Token);

            var retrievedHeaderPair = retrieved.Message.Headers.Where(i => i.Key == "value").FirstOrDefault();
            Assert.NotNull(retrievedHeaderPair);
            var retrievedValue = Encoding.UTF8.GetString(retrievedHeaderPair.GetValueBytes());
            Assert.Equal(value, retrievedValue);
        }
    }
}
