namespace ShivaEnterpriseWebApp.Model
{
    public class PurchaseReturnDetail
    {
        public int PurchaseReturnId { get; set; }
        public int PurchaseId { get; set; }
        public int VendorId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Quantity { get; set; }
        public string ReturnReason { get; set; }
        public string Status { get; set; } = "Pending";
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public PurchaseOrder PurchaseOrder { get; set; }
        public Vendor Vendor { get; set; }
    }
}
