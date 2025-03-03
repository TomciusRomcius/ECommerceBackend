using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities.StoreLocation
{
    public class StoreLocationEntity
    {
        [Range(1, int.MaxValue, ErrorMessage = "Invalid store location id!")]
        public int StoreLocationId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Display name cannot be empty")]
        public string DisplayName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address cannot be empty")]
        public string Address { get; set; }

        public StoreLocationEntity(int storeLocationId, string displayName, string address)
        {
            StoreLocationId = storeLocationId;
            DisplayName = displayName;
            Address = address;
        }
    }
}