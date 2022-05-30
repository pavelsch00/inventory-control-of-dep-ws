namespace inventory_control_of_dep_api.Models.Aprovar
{
    public class GetAprovarByInventoryBookResponse
    {
        public List<AprovarResponse> Aprovars { get; set; }

        public bool IsAprove { get; set; }
    }
}
