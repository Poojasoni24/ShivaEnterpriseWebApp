using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ShivaEnterpriseWebApp.Model
{
    public class Vendor
    {
        public Guid VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string VendorType { get; set; }
        public string VendorAddress { get; set; }
        public string Phoneno { get; set; }
        public string Email { get; set; }
        public Guid BankId { get; set; }
        public Guid TaxId { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public string? Remark { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        
        public List<SelectListItem> BankList { get; set; }
        public Bank Bank { get; set; }
        public List<SelectListItem> TaxList { get; set; }
        public Tax Tax { get; set; }
        
    }
}
