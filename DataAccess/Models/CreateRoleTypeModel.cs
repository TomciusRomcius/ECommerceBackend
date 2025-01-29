namespace ECommerce.DataAccess.Models
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