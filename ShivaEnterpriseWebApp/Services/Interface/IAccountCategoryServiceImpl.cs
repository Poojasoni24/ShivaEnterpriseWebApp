using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IAccountCategoryServiceImpl
    {
        Task<List<AccountCategory>> GetAccountCategoryList(string authToken);
        Task<AccountCategory> GetAccountCategoryById(string accountId, string authToken);
        Task<(bool successs, string message)> DeleteAccountCategory(string AccountCategoryId, string authToken);
        Task<(bool success, string message)> AddAccountCategoryDetailsAsync(AccountCategory accountCategory, string authToken);
        Task<(bool success, string message)> EditAccountCategoryDetailsAsync(AccountCategory accountCategory, string authToken);
    }
}
