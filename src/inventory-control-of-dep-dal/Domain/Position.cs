namespace inventory_control_of_dep_dal.Domain
{
    public class Position : IHasBasicId
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
