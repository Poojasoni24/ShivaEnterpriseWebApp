using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShivaEnterpriseWebApp.Model
{
    public class PurchaseOrder
    {
        public Guid PurchaseOrderId { get; set; }
        public Guid VendorID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PurchaseOrderStatus { get; set; }
        public string Doc_No { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public List<SelectListItem> VendorList { get; set; }
        public Vendor Vendor { get; set; }    }
}
