namespace inventory_control_of_dep_api.Models.Aprovar
{
    public class AprovarResponse
    {
        public string Id { get; set; }

        public int InventoryBookId { get; set; }

        public string UserId { get; set; }

        public bool IsAprove { get; set; }
    }
}
