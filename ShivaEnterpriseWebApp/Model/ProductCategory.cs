namespace ShivaEnterpriseWebApp.Model
{
    public class ProductCategory
    {
        public Guid ProductCategoryId { get; set; }
        public string ProductCategoryCode { get; set; }
        public string ProductCategoryName { get; set; }
        public string? ProductCategoryDescription { get; set; }
        public bool ProductCategoryStatus { get; set; }
    }
}
