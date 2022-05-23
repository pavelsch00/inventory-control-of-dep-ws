using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.User
{
    public class ChangeUserPasswordRequest
    {
        [Required]
        public string NewPassword { get; set; }
    }
}
