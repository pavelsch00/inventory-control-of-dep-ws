namespace inventory_control_of_dep_api.Models.User
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string LastName { get; set; }

        public IList<string> Roles { get; set; }

        public int? PositionId { get; set; }

        public int? FacultyId { get; set; }

        public int? DepartmentId { get; set; }

        public string PositionName { get; set; }

        public string FacultyName { get; set; }

        public string DepartmentName { get; set; }

        public string Email { get; set; }
        
        public bool IsActive { get; set; }
    }
}
