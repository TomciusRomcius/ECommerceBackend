namespace ECommerce.Application.src.Interfaces;

public interface IWebhookCoordinatorService
{
    Task HandlePaymentWebhook(string json, string signature);
}