using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.Position
{
    public class CreatePositionRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
