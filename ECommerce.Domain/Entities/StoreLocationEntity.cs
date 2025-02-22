namespace ECommerce.Domain.Entities.StoreLocation
{
    public class StoreLocationEntity
    {
        public int StoreLocationId { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }

        public StoreLocationEntity(int storeLocationId, string displayName, string address)
        {
            StoreLocationId = storeLocationId;
            DisplayName = displayName;
            Address = address;
        }
    }
}