using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShivaEnterpriseWebApp.Model
{
    public class SalesOrderDetail
    {
        public Guid SalesOrderDetailId { get; set; }
        public Guid SalesOrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid BrandId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal NetTotal { get; set; }
        public string Tax_Percentage { get; set; }
        public bool IsActive { get; set; }


        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public Product Product { get; set; }
        public Brand Brand { get; set; }

    }
}
