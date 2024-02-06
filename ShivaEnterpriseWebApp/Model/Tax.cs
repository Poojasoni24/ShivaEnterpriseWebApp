namespace ShivaEnterpriseWebApp.Model
{
    public class Tax 
    {
        public Guid TaxId { get; set; }
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public string? TaxDescription { get; set; }
        public bool IsActive { get; set; }
        public string TaxType { get; set; }
        public string TaxRate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
