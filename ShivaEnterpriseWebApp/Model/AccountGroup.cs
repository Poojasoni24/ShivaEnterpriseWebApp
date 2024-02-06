using System.ComponentModel;

namespace ShivaEnterpriseWebApp.Model
{
    public class AccountGroup
    {
        public Guid AccountGroupId { get; set; }
        public string AccountGroupCode { get; set; }
        public string AccountGroupName { get; set; }
        public string? AccountGroupDescription { get; set; }

        [DisplayName("Account Group Status")]
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
