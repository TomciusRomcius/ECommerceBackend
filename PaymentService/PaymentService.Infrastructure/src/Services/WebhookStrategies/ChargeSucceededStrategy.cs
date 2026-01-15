using EventSystemHelper.Kafka.Services;
using EventSystemHelper.Kafka.Utils;
using Microsoft.Extensions.Options;
using PaymentService.Domain.src.Utils;
using PaymentService.Infrastructure.src.EventTypes;
using PaymentService.Infrastructure.src.Interfaces;
using Stripe;
using System.Text.Json;
using PaymentService.Application.src.Persistence;

namespace PaymentService.Infrastructure.src.Services.WebhookStrategies
{
    public class ChargeSucceededStrategy : IStripeWebhookStrategy
    {
        private readonly KafkaConfiguration _kafkaConfiguration;

        public ChargeSucceededStrategy(IOptions<KafkaConfiguration> kafkaConfiguration)
        {
            _kafkaConfiguration = kafkaConfiguration.Value;
        }

        public string EventType => "charge.succeeded";

        public async Task<ResultError?> RunAsync(IHasObject ev)
        {
            KafkaEventProducer producer = new KafkaEventProducer(_kafkaConfiguration);
            Charge? stripeEvent = ev as Charge;

            if (stripeEvent == null)
            {
                return new ResultError(
                    ResultErrorType.INVALID_OPERATION_ERROR,
                    "Trying to run a charge succeeded strategy when given object is not a charge!"
                );
            }

            stripeEvent.Metadata.TryGetValue("userid", out string? userId);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new ResultError(
                    ResultErrorType.INVALID_OPERATION_ERROR,
                    "Charge object does not have an userid attached to its metadata!"
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
