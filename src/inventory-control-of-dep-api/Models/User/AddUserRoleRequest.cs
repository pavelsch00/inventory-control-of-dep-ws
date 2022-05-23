using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.User
{
    public class AddUserRoleRequest
    {
        [Required]
        public List<string> Roles { get; set; }
    }
}
