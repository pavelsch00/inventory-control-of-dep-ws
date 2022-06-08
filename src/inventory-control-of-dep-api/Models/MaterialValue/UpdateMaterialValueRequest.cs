using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.MaterialValue
{
    public class UpdateMaterialValueRequest
    {
        public string Description { get; set; }

        public int Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public DateTime DateOfIssue { get; set; }

        public DateTime WriteOffDate { get; set; }

        public string FactoryNumber { get; set; }

        public string InventoryNumber { get; set; }

        public string NomenclatureNumber { get; set; }

        public string PassportNumber { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int RoomId { get; set; }
    }
}
