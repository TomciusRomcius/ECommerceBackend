namespace ECommerce.DataAccess.Models.Manufacturer
{
    public class UpdateManufacturerModel
    {
        public int ManufacturerId { get; set; }
        public string? Name { get; set; }

        public UpdateManufacturerModel(int manufacturerId, string? name)
        {
            ManufacturerId = manufacturerId;
            Name = name;
        }
    }
}