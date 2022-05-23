
namespace inventory_control_of_dep_api.Infrastructure.Services.Validators.InventoryBookValidators
{
    public interface IInventoryBookValidator
    {
        Task Validate(string userId, int operationsTypeId, int materialValueId);
    }
}