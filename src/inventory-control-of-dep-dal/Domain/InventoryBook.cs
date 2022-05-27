using System;

namespace inventory_control_of_dep_dal.Domain
{
    public class InventoryBook : IHasBasicId
    {
        private DateTime _dateOfIssue;

        public int Id { get; set; }

        public int MaterialValueId { get; set; }

        public string UserId { get; set; }

        public int OperationTypeId { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get => _dateOfIssue; set => _dateOfIssue = value.ToUniversalTime(); }
    }
}
