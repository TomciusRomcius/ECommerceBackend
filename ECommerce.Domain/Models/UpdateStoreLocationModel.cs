namespace ECommerce.Domain.Models.StoreLocation
{
    public class UpdateStoreLocationModel
    {
        public int StoreLocationId { get; set; }
        public string? DisplayName { get; set; }
        public string? Address { get; set; }

        public UpdateStoreLocationModel(int storeLocationId, string? displayName, string? address)
        {
            StoreLocationId = storeLocationId;
            DisplayName = displayName;
            Address = address;
        }
    }
}