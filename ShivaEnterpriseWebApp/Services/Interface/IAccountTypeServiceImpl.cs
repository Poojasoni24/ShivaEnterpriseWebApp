using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IAccountTypeServiceImpl
    {
        Task<List<AccountType>> GetAccountTypeList(string authToken);
        Task<AccountType> GetAccountTypeById(Guid accountTypeId, string authToken);
        Task<(bool successs, string message)> DeleteAccountType(string accountTypeId, string authToken);
        Task<(bool success, string message)> AddAccountTypeDetailsAsync(AccountType accountType, string authToken);
        Task<(bool success, string message)> EditAccountTypeDetailsAsync(AccountType accountType, string authToken);
    }
}
