using ECommerce.Infrastructure.Utils;

namespace ECommerce.Infrastructure.Services
{
    public class OpenIdProviderConfigService
    {
        public Dictionary<string, OpenIdProviderConfig> _providers { get; } = new Dictionary<string, OpenIdProviderConfig>();

        public void RegisterProvider(string providerName, OpenIdProviderConfig config)
        {
            _providers.Add(providerName, config);
        }

        public OpenIdProviderConfig GetProvider(string providerName)
        {
            return _providers[providerName];
        }
    }
}