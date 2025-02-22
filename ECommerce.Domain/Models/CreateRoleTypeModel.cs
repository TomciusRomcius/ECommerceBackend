namespace ECommerce.Domain.Models.RoleType
{
    public class CreateRoleTypeModel
    {
        public string Name { get; set; }

        public CreateRoleTypeModel(string name)
        {
            Name = name;
        }
    }
}