using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ISalesReturnServiceImpl
    {
        Task<List<SalesReturn>> GetSalesReturnList(string authToken);
        Task<SalesReturn> GetSalesReturnById(Guid SalesReturnId, string authToken);
        Task<(bool successs, string message)> DeleteSalesReturn(Guid SalesReturnId, string authToken);
        Task<(bool success, string message)> AddSalesReturnDetailsAsync(SalesReturn SalesReturn, string authToken);
        Task<(bool success, string message)> EditSalesReturnDetailsAsync(SalesReturn SalesReturn, string authToken);
    }
}
