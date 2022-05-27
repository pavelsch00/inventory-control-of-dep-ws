using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.MaterialValue
{
    public class CreateMaterialValueRequest
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public DateTime DateOfIssue { get; set; }

        [Required]
        public DateTime WriteOffDate { get; set; }

        public string FactoryNumber { get; set; }

        public string InventoryNumber { get; set; }

        public string NomenclatureNumber { get; set; }

        public string PassportNumber { get; set; }

        [Required]
        public int RoomId { get; set; }
    }
}
