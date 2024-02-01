using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShivaEnterpriseWebApp.Model
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string? AccountDescription { get; set; }
        public bool AccountStatus { get; set; }
        public string AccountGroupId { get; set; }
        public string AccountTypeId { get; set; }
        public string AccountCategoryId { get; set; }


        public List<SelectListItem> AccountGroupList { get; set; }

        public AccountGroup AccountGroup { get; set; }
        public List<SelectListItem> AccountCategoryList { get; set; }

        public AccountCategory AccountCategory { get; set; }
        public List<SelectListItem> AccountTypeList { get; set; }

        public AccountType AccountType { get; set; }

    }
}
