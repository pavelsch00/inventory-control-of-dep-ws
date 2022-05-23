using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.User
{
    public class UpdateUserRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int PositionId { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}
