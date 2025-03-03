using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities.RoleType
{
    public class RoleTypeEntity
    {
        [Range(1, int.MaxValue, ErrorMessage = "Invalid RoleTypeId!")]
        public int RoleTypeId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty!")]
        public string Name { get; set; }

        public RoleTypeEntity(int roleTypeId, string name)
        {
            RoleTypeId = roleTypeId;
            Name = name;
        }
    }
}