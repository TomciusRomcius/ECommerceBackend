namespace ECommerce.Domain.Entities.Manufacturer
{
    public class ManufacturerEntity
    {
        public int ManufacturerId { get; set; }
        public string Name { get; set; }

        public ManufacturerEntity(int manufacturerId, string name)
        {
            ManufacturerId = manufacturerId;
            Name = name;
        }
    }
}