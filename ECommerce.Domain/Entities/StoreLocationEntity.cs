using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

public class StoreLocationEntity
{
    public StoreLocationEntity(int storeLocationId, string displayName, string address)
    {
        StoreLocationId = storeLocationId;
        DisplayName = displayName;
        Address = address;
    }

    [Range(1, int.MaxValue, ErrorMessage = "Invalid store location id!")]
    public int StoreLocationId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Display name cannot be empty")]
    public string DisplayName { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Address cannot be empty")]
    public string Address { get; set; }
}