namespace ShivaEnterpriseWebApp.Model
{
    public class AccountGroup
    {
        public Guid AccountGroupId { get; set; }
        public string AccountGroupCode { get; set; }
        public string AccountGroupName { get; set; }
        public string? AccountGroupDescription { get; set; }
        public bool AccountGroupStatus { get; set; }
    }
}
