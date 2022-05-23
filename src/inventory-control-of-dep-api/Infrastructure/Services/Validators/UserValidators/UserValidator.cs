using inventory_control_of_dep_api.Infrastructure.CustomExaptions;
using inventory_control_of_dep_dal.Domain;
using inventory_control_of_dep_dal.Repository;

namespace inventory_control_of_dep_api.Infrastructure.Services.Validators.UserValidators
{
    public class UserValidator : IUserValidator
    {
        private readonly IRepository<Position> _positionRepository;
        private readonly IRepository<Faculty> _facultyRepository;
        private readonly IRepository<Department> _departmentRepository;

        public UserValidator(IRepository<Position> positionRepository,
            IRepository<Faculty> facultyRepository, IRepository<Department> departmentRepository)
        {
            _positionRepository = positionRepository ?? throw new ArgumentNullException(nameof(positionRepository));
            _facultyRepository = facultyRepository ?? throw new ArgumentNullException(nameof(facultyRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        }

        public async Task Validate(int positionId, int facultyId, int departmentId)
        {
            var position = await _positionRepository.GetById(positionId);
            var faculty = await _facultyRepository.GetById(facultyId);
            var department = await _departmentRepository.GetById(departmentId);

            if (position is null)
            {
                throw new ValidationException("Position doesn't exist.");
            }

            if (faculty is null)
            {
                throw new ValidationException("Faculty doesn't exist.");
            }

            if (department is null)
            {
                throw new ValidationException("Department doesn't exist.");
            }
        }
    }
}
