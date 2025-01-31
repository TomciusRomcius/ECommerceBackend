namespace ECommerce.DataAccess.Models.StoreLocation
{
    public class StoreLocationModel
    {
        public int StoreLocationId { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }

        public StoreLocationModel(int storeLocationId, string displayName, string address)
        {
            StoreLocationId = storeLocationId;
            DisplayName = displayName;
            Address = address;
        }
    }
}