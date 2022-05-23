using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.MaterialValue
{
    public class CreateMaterialValueRequest
    {
        public string? Description { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public DateTime? DateOfIssue { get; set; }

        public DateTime? WriteOffDate { get; set; }

        public int? FactoryNumber { get; set; }

        public int? InventoryNumber { get; set; }

        public int? NomenclatureNumber { get; set; }

        public int? PassportNumber { get; set; }

        [Required]
        public int RoomId { get; set; }
    }
}
