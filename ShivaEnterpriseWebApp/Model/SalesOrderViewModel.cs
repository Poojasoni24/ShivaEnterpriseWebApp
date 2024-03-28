namespace ShivaEnterpriseWebApp.Model
{
    public class SalesOrderViewModel
    {
        public SalesOrder SalesOrder { get; set; }

        public SalesOrderDetail SODetail { get; set; }

        public Customer Customer {  get; set; }
    }
}
