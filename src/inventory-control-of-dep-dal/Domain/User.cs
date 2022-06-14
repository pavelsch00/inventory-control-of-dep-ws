using Microsoft.AspNetCore.Identity;

namespace inventory_control_of_dep_dal.Domain
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string LastName { get; set; }

        public int PositionId { get; set; }

        public int FacultyId { get; set; }

        public int DepartmentId { get; set; }

        public bool IsActive { get; set; }
    }
}
