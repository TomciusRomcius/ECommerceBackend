namespace ECommerce.Infrastructure.Utils
{
    public class OpenIdProviderConfig
    {
        public required string Authority { get; set; }
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
    }
}