namespace ECommerce.DataAccess.Models
{
    public class RoleTypeModel
    {
        public string RoleTypeId { get; set; }
        public string Name { get; set; }

        public RoleTypeModel(string roleTypeId, string name)
        {
            RoleTypeId = roleTypeId;
            Name = name;
        }
    }
}