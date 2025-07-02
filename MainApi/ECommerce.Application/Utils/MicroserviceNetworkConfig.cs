using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.Utils
{
    public class MicroserviceNetworkConfig
    {
        [Required]
        public required string PaymentServiceUrl { get; set; }
    }
}
