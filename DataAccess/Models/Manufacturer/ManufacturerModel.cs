namespace ECommerce.DataAccess.Models.Manufacturer
{
    public class ManufacturerModel
    {
        public int ManufacturerId { get; set; }
        public string Name { get; set; }

        public ManufacturerModel(int manufacturerId, string name)
        {
            ManufacturerId = manufacturerId;
            Name = name;
        }
    }
}