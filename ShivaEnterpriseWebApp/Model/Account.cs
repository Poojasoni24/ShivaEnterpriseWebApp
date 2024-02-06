using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShivaEnterpriseWebApp.Model
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string? AccountDescription { get; set; }
        public bool IsActive { get; set; }
        public Guid AccountGroupId { get; set; }
        public Guid AccountTypeId { get; set; }
        public Guid AccountCategoryId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public List<SelectListItem> AccountGroupList { get; set; }

        public AccountGroup AccountGroup { get; set; }
        public List<SelectListItem> AccountCategoryList { get; set; }

        public AccountCategory AccountCategory { get; set; }
        public List<SelectListItem> AccountTypeList { get; set; }

        public AccountType AccountType { get; set; }

    }
}
