using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ShivaEnterpriseWebApp.Model
{
    public class Brand
    {
        public Guid BrandId { get; set; }
        public string BrandCode { get; set; }
        public string BrandName { get; set; }
        public string? BrandDescription { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
