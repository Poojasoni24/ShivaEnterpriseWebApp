namespace ShivaEnterpriseWebApp.Model
{
    public class Inventory
    {
        public Guid InventoryId { get; set; }

        public string? InventoryCode { get; set; }

        public Guid ProductId { get; set; }

        public int OpeningQty { get; set; }

        public int ClosingQty { get; set; }

        public int InQuantity { get; set; }

        public int OutQuantity { get; set; }

        public decimal InventoryCost { get; set; }

        public DateTime TransactionDate { get; set; }

        public string ModifiedBy { get; set; }

    }
}
