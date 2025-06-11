namespace PaymentService.Application.Interfaces;

public interface IWebhookCoordinatorService
{
    Task HandlePaymentWebhook(string json, string signature);
}