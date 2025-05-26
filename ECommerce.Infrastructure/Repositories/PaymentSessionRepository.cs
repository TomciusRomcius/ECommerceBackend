using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;

namespace ECommerce.Infrastructure.Repositories;

public class PaymentSessionRepository : IPaymentSessionRepository
{
    private readonly IPostgresService _postgresService;

    public PaymentSessionRepository(IPostgresService postgresService)
    {
        _postgresService = postgresService;
    }

    public async Task CreatePaymentSessionAsync(PaymentSessionEntity paymentSessionEntity)
    {
        var query = @"
                INSERT INTO paymentSessions (paymentSessionId, userId, paymentSessionProvider)
                VALUES ($1, $2, $3);
            ";

        QueryParameter[] parameters =
        [
            new(paymentSessionEntity.PaymentSessionId),
            new(paymentSessionEntity.UserId),
            new(paymentSessionEntity.PaymentSessionProvider)
        ];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }

    public async Task<PaymentSessionEntity?> GetPaymentSession(Guid userId)
    {
        var query = @"
                SELECT paymentSessionId, paymentSessionProvider
                FROM paymentSessions
                WHERE userId = $1; 
            ";

        QueryParameter[] parameters =
        [
            new(userId)
        ];

        List<Dictionary<string, object>> rows = await _postgresService.ExecuteAsync(query, parameters);

        PaymentSessionEntity? result = null;

        if (rows.Count == 1)
        {
            Dictionary<string, object> row = rows[0];

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
        var query = @"
                DELETE FROM paymentSessions
                WHERE userId = $1;
            ";

        QueryParameter[] parameters = [new(userId)];

        await _postgresService.ExecuteScalarAsync(query, parameters);
    }
}