using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ShivaEnterpriseWebApp.Model
{
    public class Bank
    {
        public Guid BankId { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string? BankDescription { get; set; }
        public bool BankStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
