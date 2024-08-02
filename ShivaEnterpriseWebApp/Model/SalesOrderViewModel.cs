namespace ShivaEnterpriseWebApp.Model
{
    public class SalesOrderViewModel
    {
        public SalesOrder SalesOrder { get; set; }

        public List<SalesOrderDetail> SODetail { get; set; }
        public List<SalesOrderDetail>? UpdatedSODetail { get; set; }
    }
}
