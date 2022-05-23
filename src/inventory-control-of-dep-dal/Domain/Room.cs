namespace inventory_control_of_dep_dal.Domain
{
    public class Room : IHasBasicId
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }
    }
}
