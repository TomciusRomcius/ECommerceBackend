namespace ECommerce.Domain.Entities.RoleType
{
    public class RoleTypeEntity
    {
        public int RoleTypeId { get; set; }
        public string Name { get; set; }

        public RoleTypeEntity(int roleTypeId, string name)
        {
            RoleTypeId = roleTypeId;
            Name = name;
        }
    }
}