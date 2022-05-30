namespace inventory_control_of_dep_dal.Domain
{
    public class Aprovar : IHasBasicId
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int InventoryBookId { get; set; }

        public bool IsAprove { get; set; }
    }
}
