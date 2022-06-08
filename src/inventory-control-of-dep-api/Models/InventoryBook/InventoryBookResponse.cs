namespace inventory_control_of_dep_api.Models.InventoryBook
{
    public class InventoryBookResponse
    {
        public int Id { get; set; }

        public int MaterialValueId { get; set; }

        public string MaterialValueName { get; set; }

        public string MaterialValuInventoryNumber { get; set; }

        public string UserId { get; set; }

        public int OperationTypeId { get; set; }

        public string OperationTypeName { get; set; }

        public string Comment { get; set; }

        public string RoomNumber { get; set; }
        
        public string NomenclatureNumber { get; set; }

        public string CategoryName { get; set; }

        public bool IsAprove { get; set; }

        public DateTime Date { get; set; }
    }
}
