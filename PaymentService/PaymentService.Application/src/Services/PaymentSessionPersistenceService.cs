using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentService.Application.src.Interfaces;
using PaymentService.Application.src.Persistence;
using PaymentService.Application.src.Utils;
using PaymentService.Domain.src.Entities;
using PaymentService.Domain.src.Utils;

namespace PaymentService.Application.src.Services
{
    public class PaymentSessionPersistenceService : IPaymentSessionPersistenceService
    {
        private readonly ILogger _logger;
        private readonly DatabaseContext _databaseContext;
        private readonly IPaymentSessionFactory _paymentSessionServiceFactory;

        public PaymentSessionPersistenceService(ILogger<PaymentSessionPersistenceService> logger,
            DatabaseContext databaseContext,
            IPaymentSessionFactory paymentSessionFactory,
            IPaymentSessionFactory paymentSessionServiceFactory)
        {
            _logger = logger;
            _databaseContext = databaseContext;
            _paymentSessionServiceFactory = paymentSessionServiceFactory;
        }

        public async Task<ResultError?> CreateAsync(PaymentSessionEntity entity)
        {
            _logger.LogTrace("Entered CreateAsync");
            _logger.LogDebug("Inserting the payment session into the database: {}", JsonUtils.Serialize(entity));

            try
            {
                _databaseContext.PaymentSessions.Add(entity);
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

        public async Task<PaymentSessionEntity?> GetUserSessionAsync(Guid userId)
        {
            _logger.LogTrace("Entered GetUserSessionAsync");
            _logger.LogDebug("UserId: {}", userId);

            PaymentSessionEntity? result = await _databaseContext.PaymentSessions
                .Where(ps => ps.UserId == userId)
                .FirstOrDefaultAsync();

            _logger.LogDebug("Retrieved payment session: {}", JsonUtils.Serialize(result));
            return result;
        }
    }
}
