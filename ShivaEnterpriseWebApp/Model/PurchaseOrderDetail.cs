using System.ComponentModel.DataAnnotations;

namespace ShivaEnterpriseWebApp.Model
{
    public class PurchaseOrderDetail
    {
        public Guid PurchaseOrderDetailId { get; set; }
        public Guid PurchaseOrderId { get; set; }

        [Required(ErrorMessage = "Product is required")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        public Guid BrandId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "UnitPrice is required")]
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
