using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ShivaEnterpriseWebApp.Model
{
    public class AccountCategory
    {
        public Guid AccountCategoryId { get; set; }
        public string AccountCategoryCode { get; set; }
        public string AccountCategoryName { get; set; }
        public string? AccountCategoryDescription { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
