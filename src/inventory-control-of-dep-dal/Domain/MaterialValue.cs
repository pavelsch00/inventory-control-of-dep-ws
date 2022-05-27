using System;

namespace inventory_control_of_dep_dal.Domain
{
    public class MaterialValue : IHasBasicId
    {
        private DateTime _writeOffDate;
        private DateTime _dateOfIssue;

        public int Id { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int CategoryId { get; set; }

        public DateTime DateOfIssue { get => _dateOfIssue; set => _dateOfIssue = value.ToUniversalTime(); }

        public DateTime WriteOffDate  { get => _writeOffDate; set => _writeOffDate = value.ToUniversalTime(); }

        public string FactoryNumber { get; set; }

        public string InventoryNumber { get; set; }

        public string NomenclatureNumber { get; set; }

        public string PassportNumber { get; set; }

        public int RoomId { get; set; }
    }
}
