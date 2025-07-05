using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.src.Entities;

public class ManufacturerEntity
{
    public ManufacturerEntity(string name)
    {
        ManufacturerId = -1;
        Name = name;
    }

    public ManufacturerEntity(int manufacturerId, string name)
    {
        ManufacturerId = manufacturerId;
        Name = name;
    }

    public int ManufacturerId { get; set; }

    public string Name { get; set; }
}