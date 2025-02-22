using ECommerce.DataAccess.Services;
using ECommerce.DataAccess.Utils;
using ECommerce.DataAccess.Utils.DictionaryExtensions;
using ECommerce.Domain.Entities.PaymentSession;
using ECommerce.Domain.Repositories.PaymentSession;

namespace ECommerce.DataAccess.Repositories.PaymentSession
{
    public class PaymentSessionRepository : IPaymentSessionRepository
    {
        readonly IPostgresService _postgresService;

        public PaymentSessionRepository(IPostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        public async Task CreatePaymentSessionAsync(PaymentSessionEntity paymentSessionEntity)
        {
            string query = @"
                INSERT INTO paymentSessions (paymentSessionId, userId, paymentSessionProvider)
                VALUES ($1, $2, $3);
            ";

            QueryParameter[] parameters = [
                new(paymentSessionEntity.PaymentSessionId),
                new(paymentSessionEntity.UserId),
                new(paymentSessionEntity.PaymentSessionProvider),
            ];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }
        public async Task<PaymentSessionEntity?> GetPaymentSession(Guid userId)
        {
            string query = @"
                SELECT paymentSessionId, paymentSessionProvider
                FROM paymentSessions
                WHERE userId = $1; 
            ";

            QueryParameter[] parameters = [
                new(userId),
            ];

            List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);

            PaymentSessionEntity? result = null;

            if (rows.Count == 1)
            {
                var row = rows[0];

                result = new PaymentSessionEntity(
                    row.GetColumn<string>("paymentsessionid"),
                    userId,
                    row.GetColumn<string>("paymentsessionprovider")
                );
            }

            return result;
        }

        public async Task DeletePaymentSession(Guid userId)
        {
            string query = @"
                DELETE FROM paymentSessions
                WHERE userId = $1;
            ";

            QueryParameter[] parameters = [new QueryParameter(userId)];

            await _postgresService.ExecuteScalarAsync(query, parameters);
        }
    }
}