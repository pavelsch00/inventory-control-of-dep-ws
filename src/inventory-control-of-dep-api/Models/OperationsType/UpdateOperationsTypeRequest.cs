using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.OperationsType
{
    public class UpdateOperationsTypeRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
