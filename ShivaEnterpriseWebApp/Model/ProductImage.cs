namespace ShivaEnterpriseWebApp.Model
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImageURL { get; set; }        
        public Product product { get; set; }
    }
}
