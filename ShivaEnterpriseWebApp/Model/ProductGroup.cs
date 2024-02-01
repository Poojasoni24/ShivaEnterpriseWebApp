namespace ShivaEnterpriseWebApp.Model
{
    public class ProductGroup
    {
        public Guid ProductGroupId { get; set; }
        public string ProductGroupCode { get; set; }
        public string ProductGroupName { get; set; }
        public string? ProductGroupDescription { get; set; }
        public bool ProductGroupStatus { get; set; }
    }
}
