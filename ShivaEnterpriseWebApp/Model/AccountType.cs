namespace ShivaEnterpriseWebApp.Model
{
    public class AccountType
    {
        public Guid AccountTypeId { get; set; }
        public string AccountTypeCode { get; set; }
        public string AccountTypeName { get; set; }
        public string? AccountTypeDescription { get; set; }
        public bool AccountTypeStatus { get; set; }
    }
}
