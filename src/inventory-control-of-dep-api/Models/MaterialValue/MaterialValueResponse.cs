namespace inventory_control_of_dep_api.Models.MaterialValue
{
    public class MaterialValueResponse
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        public int Price { get; set; }

        public int CategoryId { get; set; }

        public DateTime? DateOfIssue { get; set; }

        public DateTime? WriteOffDate { get; set; }

        public int? FactoryNumber { get; set; }

        public int? InventoryNumber { get; set; }

        public int? NomenclatureNumber { get; set; }

        public int? PassportNumber { get; set; }

        public int RoomId { get; set; }
    }
}
