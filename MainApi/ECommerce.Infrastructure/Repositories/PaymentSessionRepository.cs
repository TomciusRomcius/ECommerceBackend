using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Utils;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Utils;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace ECommerce.Infrastructure.Repositories;

public class PaymentSessionRepository : IPaymentSessionRepository
{
    private readonly IPostgresService _postgresService;
    private readonly ILogger<PaymentSessionRepository> _logger;

    public PaymentSessionRepository(IPostgresService postgresService, ILogger<PaymentSessionRepository> logger)
    {
        _postgresService = postgresService;
        _logger = logger;
    }

    public async Task<ResultError?> CreatePaymentSessionAsync(PaymentSessionEntity paymentSessionEntity)
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

        try
        {
            await _postgresService.ExecuteScalarAsync(query, parameters);
        }
        catch (NpgsqlException ex)
        {
            if (ex is PostgresException)
            {
                if (ex.SqlState == PostgresErrorCodes.UniqueViolation)
                {
                    return new ResultError(
                        ResultErrorType.INVALID_OPERATION_ERROR,
                        "Trying to create a payment session when there is an ongoing session"
                    );
                }

                _logger.LogError("Encountered an unknown database error: {}", ex);
                return new ResultError(ResultErrorType.UNKNOWN_ERROR, "Encountered an unknown database error");
            }
        }

        catch (Exception ex)
        {
            _logger.LogError("Encountered an unknown exceptiopn: {}", ex);
            return new ResultError(ResultErrorType.UNKNOWN_ERROR, "Unknown error");
        }

        return null;
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