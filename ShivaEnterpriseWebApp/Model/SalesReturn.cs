using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.NetworkInformation;

namespace ShivaEnterpriseWebApp.Model
{
    public class SalesReturn
    {
        public Guid SalesReturnID { get; set; }
        public Guid SalesOrderID { get; set; }
        public DateTime ReturnDate { get; set; }
        public string ReasonForReturn { get; set; }
        public int ReturnedQuantity { get; set; }
        public decimal? RestockingFee { get; set; }
        public string Comments { get; set; }

        public SalesOrder SalesOrder { get; set; }
    }
}
