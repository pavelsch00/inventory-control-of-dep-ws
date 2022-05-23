using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.Category
{
    public class UpdateCategoryRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
