using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.src.Entities;

public class RoleTypeEntity
{
    public RoleTypeEntity(int roleTypeId, string name)
    {
        RoleTypeId = roleTypeId;
        Name = name;
    }

    [Key]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid RoleTypeId!")]
    public int RoleTypeId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty!")]
    public string Name { get; set; }
}