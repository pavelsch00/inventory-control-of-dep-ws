using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.User
{
    public class SetIsActiveRequest
    {
        [Required]
        public bool IsActive { get; set; }
    }
}
