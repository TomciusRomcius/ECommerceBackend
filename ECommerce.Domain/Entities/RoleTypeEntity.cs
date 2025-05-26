using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

public class RoleTypeEntity
{
    public RoleTypeEntity(int roleTypeId, string name)
    {
        RoleTypeId = roleTypeId;
        Name = name;
    }

    [Range(1, int.MaxValue, ErrorMessage = "Invalid RoleTypeId!")]
    public int RoleTypeId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty!")]
    public string Name { get; set; }
}