
namespace inventory_control_of_dep_api.Infrastructure.Services.Validators.MaterialValueValidators
{
    public interface IMaterialValueValidator
    {
        Task Validate(int categoryId, int roomId);
    }
}