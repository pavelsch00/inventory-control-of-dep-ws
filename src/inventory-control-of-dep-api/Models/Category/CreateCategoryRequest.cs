using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.Category
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
