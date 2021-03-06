using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.InventoryBook
{
    public class UpdateInventoryBookRequest
    {
        [Required]
        public int MaterialValueId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int OperationTypeId { get; set; }

        public string Comment { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
