namespace ECommerce.PaymentSession
{
    public class GeneratePaymentSessionOptions
    {
        public string UserId { get; set; }
        /// <summary>
        /// Price in cents
        /// </summary>
        public int Price { get; set; }
    }
}