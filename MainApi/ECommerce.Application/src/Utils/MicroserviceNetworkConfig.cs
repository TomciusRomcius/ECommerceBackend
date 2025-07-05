using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.src.Utils
{
    public class MicroserviceNetworkConfig
    {
        [Required]
        public required string PaymentServiceUrl { get; set; }
    }
}
