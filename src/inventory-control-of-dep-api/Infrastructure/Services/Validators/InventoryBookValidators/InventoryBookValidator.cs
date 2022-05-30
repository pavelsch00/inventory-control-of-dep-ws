using Microsoft.AspNetCore.Identity;

using inventory_control_of_dep_api.Infrastructure.CustomExaptions;
using inventory_control_of_dep_dal.Domain;
using inventory_control_of_dep_dal.Repository;

namespace inventory_control_of_dep_api.Infrastructure.Services.Validators.InventoryBookValidators
{
    public class InventoryBookValidator : IInventoryBookValidator
    {
        private readonly IRepository<OperationsType> _operationsTypeRepository;
        private readonly IRepository<MaterialValue> _materialValueRepository;
        private readonly UserManager<User> _userManager;

        public InventoryBookValidator(IRepository<OperationsType> operationsTypeRepository,
            IRepository<MaterialValue> materialValueRepository, UserManager<User> userManager)
        {
            _operationsTypeRepository = operationsTypeRepository ?? throw new ArgumentNullException(nameof(operationsTypeRepository));
            _materialValueRepository = materialValueRepository ?? throw new ArgumentNullException(nameof(materialValueRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task Validate(string userId, int operationsTypeId, int materialValueId)
        {
            var operationsType = await _operationsTypeRepository.GetById(operationsTypeId);
            var materialValue = await _materialValueRepository.GetById(materialValueId);
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new ValidationException("User doesn't exist.");
            }

            if (operationsType is null)
            {
                throw new ValidationException("OperationsType doesn't exist.");
            }

            if (materialValue is null)
            {
                throw new ValidationException("MaterialValue doesn't exist.");
            }
        }
    }
}
