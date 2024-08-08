namespace ShivaEnterpriseWebApp.Model
{
    public class Stock
    {
        public Guid StockId { get; set; }
        public string? StockCode { get; set; }
        public Guid ProductId { get; set; }
        public int QuantityOnHand { get; set; }
        public string? ReorderLevel { get; set; }
        public string ModifiedBy { get; set; }
    }
}
