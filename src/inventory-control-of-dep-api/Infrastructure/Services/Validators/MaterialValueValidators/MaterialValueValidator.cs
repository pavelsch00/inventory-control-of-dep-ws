using inventory_control_of_dep_api.Infrastructure.CustomExaptions;
using inventory_control_of_dep_dal.Domain;
using inventory_control_of_dep_dal.Repository;

namespace inventory_control_of_dep_api.Infrastructure.Services.Validators.MaterialValueValidators
{
    public class MaterialValueValidator : IMaterialValueValidator
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Room> _roomRepository;

        public MaterialValueValidator(IRepository<Category> categoryRepository, IRepository<Room> roomRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        }

        public async Task Validate(int categoryId, int roomId)
        {
            var category = await _categoryRepository.GetById(categoryId);
            var room = await _roomRepository.GetById(roomId);

            if (category is null)
            {
                throw new ValidationException("Category doesn't exist.");
            }

            if (room is null)
            {
                throw new ValidationException("Room doesn't exist.");
            }
        }
    }
}
