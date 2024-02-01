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
        public bool AccountCategoryStatus { get; set; }
    }
}
