﻿using ECommerce.Application.src.Utils;
using ECommerce.Domain.src.Interfaces.Services;
using ECommerce.Domain.src.Models;
using ECommerce.Domain.src.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
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

        public async Task<Result<PaymentSessionModel>> GeneratePaymentSessionAsync(GeneratePaymentSessionOptions sessionOptions)
        {
            _logger.LogTrace("Entered PaymentSessionService.GeneratePaymentSessionAsync");
            _logger.LogDebug("Creating session with options: {@PaymentSessionOptions}", sessionOptions);

            var httpContent = new StringContent(JsonUtils.Serialize(sessionOptions), Encoding.UTF8, "application/json");

            HttpResponseMessage? response = await _httpClient.PostAsync($"{_networkConfig.PaymentServiceUrl}/paymentsession", httpContent);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Failed to create payment session. HTTP body: {}. HTTP Status code: {}",
                    await response.Content.ReadAsStringAsync(),
                    response.StatusCode
                );
                return new Result<PaymentSessionModel>([new ResultError(ResultErrorType.UNKNOWN_ERROR, "Failed to create payment session")]);
            }

            string json = await response.Content.ReadAsStringAsync();
            _logger.LogDebug("Payment session json: {}", json);
            PaymentSessionModel? session = JsonUtils.Deserialize<PaymentSessionModel>(json);
            if (session == null)
            {
                _logger.LogError("Failed to create payment session. Payment service response was either empty or current response model is malformed");
                return new Result<PaymentSessionModel>([new ResultError(ResultErrorType.UNKNOWN_ERROR, "Failed to create payment session")]);
            }

            return new Result<PaymentSessionModel>(session);
        }

        public async Task<Result<PaymentSessionModel?>> GetPaymentSessionAsync(Guid userId)
        {
            _logger.LogTrace("Entered PaymentSessionService.GetPaymentSessionAsync");
            HttpResponseMessage? response = await _httpClient.GetAsync($"{_networkConfig.PaymentServiceUrl}/paymentsession?userId={userId.ToString()}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Failed to get user payment session. UserId: {}. Http body: {}. Http status code: {}.",
                    userId,
                    await response.Content.ReadAsStringAsync(),
                    response.StatusCode
                );
                return new Result<PaymentSessionModel?>([new ResultError(ResultErrorType.UNKNOWN_ERROR, "Failed to create payment session")]);
            }
            string json = await response.Content.ReadAsStringAsync();
            PaymentSessionModel? session = JsonSerializer.Deserialize<PaymentSessionModel>(json);
            return new Result<PaymentSessionModel?>(session);
        }
    }
}
