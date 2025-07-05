using Microsoft.Extensions.Logging;
using PaymentService.Application.src.Interfaces;
using PaymentService.Application.src.Persistence;
using PaymentService.Domain.src.Entities;
using PaymentService.Domain.src.Enums;
using PaymentService.Domain.src.Models;
using PaymentService.Domain.src.Utils;

namespace PaymentService.Application.src.Services
{
    public class PaymentSessionService : IPaymentSessionService
    {
        private readonly ILogger _logger;
        private readonly DatabaseContext _databaseContext;
        private readonly IPaymentSessionFactory _paymentSessionServiceFactory;

        public PaymentSessionService(ILogger<PaymentSessionService> logger,
            DatabaseContext databaseContext,
            IPaymentSessionFactory paymentSessionFactory,
            IPaymentSessionFactory paymentSessionServiceFactory)
        {
            _logger = logger;
            _databaseContext = databaseContext;
            _paymentSessionServiceFactory = paymentSessionServiceFactory;
        }

        public async Task<ResultError?> CreateAsync(GeneratePaymentSessionOptions options, PaymentProvider provider)
        {
            _logger.LogTrace("Entered CreateAsync");
            _logger.LogDebug("Creating payment session for user: {}", options.UserId);
            var paymentProviderSessionService = _paymentSessionServiceFactory.CreatePaymentSessionService(provider);

            PaymentProviderSession paymentSession = await paymentProviderSessionService.GeneratePaymentSession(options);
            _logger.LogDebug("Created payment session: {}", paymentSession);
            var sessionEntity = new PaymentSessionEntity
            {
                PaymentSessionId = paymentSession.SessionId,
                UserId = options.UserId,
                PaymentSessionProvider = provider
            };

            _logger.LogDebug("Created session entity: {}", sessionEntity);

            try
            {
                _logger.LogDebug("Inserting the payment session into the database");
                _databaseContext.PaymentSessions.Add(sessionEntity);
                await _databaseContext.SaveChangesAsync();
                _logger.LogDebug("Succesfully inserted the payment session into the database");
            }
            catch (Exception ex)
            {
                _logger.LogError("Caught an exception when trying to save payment session to the database: {}", ex);
                return new ResultError(ResultErrorType.UNKNOWN_ERROR, "Unknown error occurred when trying to save payment session to the database");
            }

            return null;
        }
    }
}
