namespace ECommerce.Manufacturers
{
    public class ManufacturerModel
    {
        public int ManufacturerId { get; set; }
        public string Name { get; set; }

        public ManufacturerModel(int id, string name)
        {
            ManufacturerId = id;
            Name = name;
        }
    }
}