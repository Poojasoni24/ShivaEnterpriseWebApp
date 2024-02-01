namespace ShivaEnterpriseWebApp.Model
{
    public class ProductType
    {
        public Guid ProductTypeId { get; set; }
        public string ProductTypeCode { get; set; }
        public string ProductTypeName { get; set; }
        public string? ProductTypeDescription { get; set; }
        public bool ProductTypeStatus { get; set; }
    }
}
