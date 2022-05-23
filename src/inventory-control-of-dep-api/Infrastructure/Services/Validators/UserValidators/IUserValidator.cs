
namespace inventory_control_of_dep_api.Infrastructure.Services.Validators.UserValidators
{
    public interface IUserValidator
    {
        Task Validate(int positionId, int facultyId, int departmentId);
    }
}