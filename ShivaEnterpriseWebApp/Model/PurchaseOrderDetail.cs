namespace ShivaEnterpriseWebApp.Model
{
    public class PurchaseOrderDetail
    {
        public Guid PurchaseOrderDetailId { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid BrandId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal NetTotal { get; set; }
        public decimal Tax_Percentage { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public Product Product { get; set; }
        public Brand Brand { get; set; }

    }
}
