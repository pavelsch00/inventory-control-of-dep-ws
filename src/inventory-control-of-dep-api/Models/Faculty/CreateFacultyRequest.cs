using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.Faculty
{
    public class CreateFacultyRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
