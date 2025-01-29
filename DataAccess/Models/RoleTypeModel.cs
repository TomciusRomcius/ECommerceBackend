namespace ECommerce.DataAccess.Models
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