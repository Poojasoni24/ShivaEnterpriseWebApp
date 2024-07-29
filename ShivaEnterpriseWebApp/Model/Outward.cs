namespace ShivaEnterpriseWebApp.Model
{
    public class Outward
    {
        public Guid OutwardId { get; set; }
        public Guid SalesOrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string ShippedBy { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal QuantityShipped { get; set; }
        public string UnitOfMeasure { get; set; }
        public string BatchNumber { get; set; }
        public int CarrierId { get; set; }
        public string CarrierName { get; set; }
        public string TrackingNumber { get; set; }
        public string ShippingMethod { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal CostPerUnit { get; set; }
        public decimal TotalCost { get; set; }
        public string Currency { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Doc_No { get; set; }
        //public virtual Customer Customer { get; set; }
        //public virtual SalesOrder SalesOrder { get; set; }
        //public virtual Product Product { get; set; }

    }
}
