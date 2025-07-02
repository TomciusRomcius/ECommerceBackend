using ECommerce.Application.UseCases.PaymentSession.Commands;
using ECommerce.Application.Utils;
using ECommerce.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;

namespace ECommerce.Application.UseCases.PaymentSession.Handlers;

public class DeletePaymentSessionHandler : IRequestHandler<DeletePaymentSessionCommand>
{
    private readonly MicroserviceNetworkConfig _microserviceNetworkConfig;
    private readonly IPaymentSessionRepository _paymentSessionRepository;
    private readonly IHttpClientFactory _httpClientFactory;

    public DeletePaymentSessionHandler(IOptions<MicroserviceNetworkConfig> microserviceNetworkConfig, IPaymentSessionRepository paymentSessionRepository, IHttpClientFactory httpClientFactory)
    {
        _microserviceNetworkConfig = microserviceNetworkConfig.Value;
        _paymentSessionRepository = paymentSessionRepository;
        _httpClientFactory = httpClientFactory;
    }

    public async Task Handle(DeletePaymentSessionCommand request, CancellationToken cancellationToken)
    {
        HttpClient client = _httpClientFactory.CreateClient();
        await client.DeleteAsync($"{_microserviceNetworkConfig.PaymentServiceUrl}/paymentsession?userId={request.UserId}");
    }
}