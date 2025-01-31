using System.ComponentModel.DataAnnotations;

namespace ECommerce.StoreLocation
{
    public class RequestRemoveLocationDto
    {
        [Required]
        public int StoreLocationId { get; set; }
    }
}