using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

public class ManufacturerEntity
{
    public ManufacturerEntity(int manufacturerId, string name)
    {
        ManufacturerId = manufacturerId;
        Name = name;
    }

    [Required(ErrorMessage = "ManufacturerId is required!")]
    public int ManufacturerId { get; set; }

    [Length(2, 50, ErrorMessage = "Manufacturer name length must be between 2 and 50 characters long")]
    public string Name { get; set; }
}