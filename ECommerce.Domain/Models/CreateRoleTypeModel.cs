namespace ECommerce.Domain.Models;

public class CreateRoleTypeModel
{
    public CreateRoleTypeModel(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}