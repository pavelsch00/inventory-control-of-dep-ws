using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.Authorization
{
    public class LoginRequst
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
