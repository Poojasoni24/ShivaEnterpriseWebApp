using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IBankServiceImpl
    {
        Task<List<Bank>> GetBankList(string authToken);
        Task<Bank> GetBankById(Guid BankId, string authToken);
        Task<(bool successs, string message)> DeleteBank(string BankId, string authToken);
        Task<(bool success, string message)> AddBankDetailsAsync(Bank Bank, string authToken);
        Task<(bool success, string message)> EditBankDetailsAsync(Bank Bank, string authToken);
    }
}
