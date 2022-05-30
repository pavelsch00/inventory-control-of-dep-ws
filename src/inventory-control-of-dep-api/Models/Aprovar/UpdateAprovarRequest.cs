using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.Aprovar
{
    public class UpdateAprovarRequest
    {
        [Required]
        public int InventoryBookId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public bool IsAprove { get; set; }
    }
}
