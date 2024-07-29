namespace ShivaEnterpriseWebApp.Model
{
    public class SalesReturnViewModel
    {
        public SalesReturn SalesReturn { get; set; }

        public List<SalesReturnDetail> SODetail { get; set; }
    
        public Customer Customer {  get; set; }
        public List<SalesReturnDetail>? UpdatedSODetail { get; set; }
    }
}
