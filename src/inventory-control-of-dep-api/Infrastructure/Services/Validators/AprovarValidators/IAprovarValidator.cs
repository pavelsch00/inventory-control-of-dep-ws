
namespace inventory_control_of_dep_api.Infrastructure.Services.Validators.AprovarValidators
{
    public interface IAprovarValidator
    {
        Task Validate(string userId, int inventoryBookId);
    }
}