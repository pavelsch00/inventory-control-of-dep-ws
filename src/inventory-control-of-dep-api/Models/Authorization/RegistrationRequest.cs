using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.Authorization
{
    public class RegistrationRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordConfirm { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public List<string> Roles { get; set; }

        [Required]
        public int PositionId { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}
