namespace ShivaEnterpriseWebApp.Model
{
    public class Unit
    {
        public Guid UnitId { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string? UnitDescription { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
