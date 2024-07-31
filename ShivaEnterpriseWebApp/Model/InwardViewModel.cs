
namespace ShivaEnterpriseWebApp.Model
{
    public class InwardViewModel
    {
        public Guid InwardId { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string ReceivedBy { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal QuantityReceived { get; set; }
        public string UnitOfMeasure { get; set; }
        public string BatchNumber { get; set; }
        public string QualityCheckStatus { get; set; }
        public string QualityCheckRemarks { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal CostPerUnit { get; set; }
        public decimal TotalCost { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public List<Product> Products { get; set; }
        public List<PurchaseOrder> PurchaseOrders { get; set; }
        public List<Vendor> Vendors { get; set; }
    }
}
