namespace ECommerce.Domain.Models.RoleType
{
    public class UpdateRoleTypeModel
    {
        public int RoleTypeId { get; set; }
        public string? Name { get; set; }

        public UpdateRoleTypeModel(int roleTypeId, string? name)
        {
            RoleTypeId = roleTypeId;
            Name = name;
        }
    }
}