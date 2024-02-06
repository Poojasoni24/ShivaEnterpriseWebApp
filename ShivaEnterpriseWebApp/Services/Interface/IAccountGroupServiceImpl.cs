using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IAccountGroupServiceImpl
    {
        Task<List<AccountGroup>> GetAccountGroupList(string authToken);
        Task<AccountGroup> GetAccountGroupById(Guid accountGroupId, string authToken);
        Task<(bool successs, string message)> DeleteAccountGroup(string AccountGroupId, string authToken);
        Task<(bool success, string message)> AddAccountGroupDetailsAsync(AccountGroup accountGroup, string authToken);
        Task<(bool success, string message)> EditAccountGroupDetailsAsync(AccountGroup accountGroup, string authToken);
    }
}
