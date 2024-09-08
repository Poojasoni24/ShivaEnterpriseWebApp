using System;

namespace ShivaEnterpriseWebApp.Model
{
    public class PurchaseOrderReturn
    {
        //public int VendorId { get; set; }
        public Guid PurchaseReturnId { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public Guid VendorId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ReturnReason { get; set; }
        public int ReturnQuantity { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = "Pending";
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; } = DateTime.Now;

        public Guid ProductId { get; set; }
        public Guid BrandId { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }

    }

}
