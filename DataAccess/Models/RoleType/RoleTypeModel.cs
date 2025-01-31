namespace ECommerce.DataAccess.Models.RoleType
{
    public class RoleTypeModel
    {
        public int RoleTypeId { get; set; }
        public string Name { get; set; }

        public RoleTypeModel(int roleTypeId, string name)
        {
            RoleTypeId = roleTypeId;
            Name = name;
        }
    }
}