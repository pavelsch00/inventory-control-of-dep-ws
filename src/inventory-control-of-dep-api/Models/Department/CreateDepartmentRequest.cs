using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.Department
{
    public class CreateDepartmentRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
