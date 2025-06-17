using EventSystemHelper.Kafka.Services;
using EventSystemHelper.Kafka.Utils;
using PaymentService.Domain.Utils;
using PaymentService.Infrastructure.EventTypes;
using PaymentService.Infrastructure.Interfaces;
using Stripe;
using System.Text.Json;

namespace PaymentService.Infrastructure.Services.WebhookStrategies
{
    public class ChargeSucceededStrategy : IStripeWebhookStrategy
    {
        private readonly KafkaConfiguration _kafkaConfiguration;

        public ChargeSucceededStrategy(KafkaConfiguration kafkaConfiguration)
        {
            _kafkaConfiguration = kafkaConfiguration;
        }

        public string EventType => "charge.succeeded";

        public async Task<ResultError?> RunAsync(IHasObject ev)
        {
            KafkaEventProducer producer = new KafkaEventProducer(_kafkaConfiguration);
            var stripeEvent = ev as Charge;

            if (stripeEvent is not Charge)
            {
                return new ResultError(
                    ResultErrorType.INVALID_OPERATION_ERROR,
                    "Trying to run a charge succeeded strategy when given object is not a charge!"
                );
            }
            stripeEvent.Metadata.TryGetValue("userid", out string? userId);
            if (String.IsNullOrWhiteSpace(userId))
            {
                return new ResultError(
                    ResultErrorType.INVALID_OPERATION_ERROR,
                    "Charge object does not have an userid attatched to its metadata!"
                );
            }

            var kafkaEvent = new ChargeSucceededEvent
            {
                UserId = userId,
                Ammount = stripeEvent.Amount,
            };

            var jsonMessage = JsonSerializer.Serialize(kafkaEvent);

            await producer.ProduceEventAsync("charge-succeeded", jsonMessage, CancellationToken.None);
            return null;
        }
    }
}
