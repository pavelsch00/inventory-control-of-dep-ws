using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.Position
{
    public class UpdatePositionRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
