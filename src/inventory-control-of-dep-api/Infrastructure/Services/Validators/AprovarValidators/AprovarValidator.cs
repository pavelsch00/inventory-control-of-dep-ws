using Microsoft.AspNetCore.Identity;

using inventory_control_of_dep_api.Infrastructure.CustomExaptions;
using inventory_control_of_dep_dal.Domain;
using inventory_control_of_dep_dal.Repository;

namespace inventory_control_of_dep_api.Infrastructure.Services.Validators.AprovarValidators
{
    public class AprovarValidator : IAprovarValidator
    {
        private readonly IRepository<InventoryBook> _inventoryBookRepository;
        private readonly UserManager<User> _userManager;

        public AprovarValidator(IRepository<InventoryBook> inventoryBookRepository, UserManager<User> userManager)
        {
            _inventoryBookRepository = inventoryBookRepository ?? throw new ArgumentNullException(nameof(inventoryBookRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task Validate(string userId, int inventoryBookId)
        {
            var inventoryBook = await _inventoryBookRepository.GetById(inventoryBookId);
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new ValidationException("User doesn't exist.");
            }

            if (inventoryBook is null)
            {
                throw new ValidationException("InventoryBook doesn't exist.");
            }
        }
    }
}
