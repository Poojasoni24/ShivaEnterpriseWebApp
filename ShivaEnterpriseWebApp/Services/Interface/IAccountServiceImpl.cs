using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IAccountServiceImpl
    {
        Task<List<Account>> GetAccountList(string authToken);
        Task<Account> GetAccountById(Guid accountId, string authToken);
        Task<(bool successs, string message)> DeleteAccount(string AccountId, string authToken);
        Task<(bool success, string message)> AddAccountDetailsAsync(Account account, string authToken);
        Task<(bool success, string message)> EditAccountDetailsAsync(Account account, string authToken);
    }
}
