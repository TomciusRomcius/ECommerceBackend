using ECommerce.Application.Utils;
using ECommerce.Domain.Interfaces.Services;
using ECommerce.Domain.Models.PaymentSession;
using ECommerce.Domain.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ECommerce.Infrastructure.src.Services
{
    public class PaymentSessionService : IPaymentSessionService
    {
        private readonly HttpClient _httpClient;
        private readonly MicroserviceNetworkConfig _networkConfig;
        private readonly ILogger<PaymentSessionService> _logger;

        public PaymentSessionService(HttpClient httpClient, IOptions<MicroserviceNetworkConfig> networkConfig, ILogger<PaymentSessionService> logger)
        {
            _httpClient = httpClient;
            _networkConfig = networkConfig.Value;
            _logger = logger;
        }

        public async Task<Result<PaymentProviderSession>> GeneratePaymentSessionAsync(GeneratePaymentSessionOptions sessionOptions)
        {
            var httpContent = new StringContent(JsonUtils.Serialize(sessionOptions));
            HttpResponseMessage? response = await _httpClient.PostAsync($"{_networkConfig.PaymentServiceUrl}/paymentsession", httpContent);
            if (!response.IsSuccessStatusCode)
            {
                return new Result<PaymentProviderSession>([new ResultError(ResultErrorType.UNKNOWN_ERROR, "Failed to create payment session")]);
            }
            string json = await response.Content.ReadAsStringAsync();
            PaymentProviderSession? session = JsonSerializer.Deserialize<PaymentProviderSession>(json);
            if (session == null)
            {
                _logger.LogError("Failed to create payment session. Payment service response was either empty or current response model is malformed");
                return new Result<PaymentProviderSession>([new ResultError(ResultErrorType.UNKNOWN_ERROR, "Failed to create payment session")]);
            }
            return new Result<PaymentProviderSession>(session);
        }

        public async Task<Result<PaymentProviderSession?>> GetPaymentSessionAsync(Guid userId)
        {
            HttpResponseMessage? response = await _httpClient.GetAsync($"{_networkConfig.PaymentServiceUrl}/paymentsession?userId={userId.ToString()}");
            if (!response.IsSuccessStatusCode)
            {
                return new Result<PaymentProviderSession?>([new ResultError(ResultErrorType.UNKNOWN_ERROR, "Failed to create payment session")]);
            }
            string json = await response.Content.ReadAsStringAsync();
            PaymentProviderSession? session = JsonSerializer.Deserialize<PaymentProviderSession>(json);
            return new Result<PaymentProviderSession?>(session);
        }
    }
}
