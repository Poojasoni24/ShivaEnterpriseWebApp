using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace ShivaEnterpriseWebApp.Model
{
    public class Branch
    {
        public Guid Branch_Id { get; set; }
        public string Branch_Code { get; set; }
        public string Branch_Name { get; set; }
        public Guid Company_Id { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public List<SelectListItem> companyList { get; set; }
        public Company Company { get; set; }
    }
}
