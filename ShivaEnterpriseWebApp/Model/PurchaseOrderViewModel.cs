namespace ShivaEnterpriseWebApp.Model
{
    public class PurchaseOrderViewModel
    {
        public PurchaseOrder PurchaseOrder { get; set; }    

        public List<PurchaseOrderDetail> PODetail { get; set; }
        public List<PurchaseOrderDetail>? UpdatedPODetail { get; set; }
    }
}
