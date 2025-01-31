namespace ECommerce.DataAccess.Models.StoreLocation
{
    public class CreateStoreLocationModel
    {
        public string DisplayName { get; set; }
        public string Address { get; set; }

        public CreateStoreLocationModel(string displayName, string address)
        {
            DisplayName = displayName;
            Address = address;
        }
    }
}