namespace ECommerce.Domain.Models;

public class UpdateRoleTypeModel
{
    public UpdateRoleTypeModel(int roleTypeId, string? name)
    {
        RoleTypeId = roleTypeId;
        Name = name;
    }

    public int RoleTypeId { get; set; }
    public string? Name { get; set; }
}