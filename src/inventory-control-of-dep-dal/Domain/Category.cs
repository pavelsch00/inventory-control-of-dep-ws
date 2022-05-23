namespace inventory_control_of_dep_dal.Domain
{
    public class Category : IHasBasicId
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
