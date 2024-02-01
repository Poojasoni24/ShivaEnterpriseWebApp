using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ShivaEnterpriseWebApp.Model
{
    public class SalesmanAgent
    {
        public Guid SalesmanAgentID { get; set; }
        public string Salesmancode { get; set; }
        public string SalesmanName { get; set; }
        public string Salesmanemail { get; set; }
        public string Salesmanphone { get; set; }
        public bool SalesmanStatus { get; set; }
    }
}
