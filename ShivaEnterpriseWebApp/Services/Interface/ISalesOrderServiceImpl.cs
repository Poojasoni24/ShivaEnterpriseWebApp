using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ISalesOrderServiceImpl
    {
        Task<List<SalesOrder>> GetSalesOrderList(string authToken);
        Task<SalesOrder> GetSalesOrderById(Guid salesorderId, string authToken);
        Task<(bool successs, string message)> DeleteSalesOrder(Guid salesorderId, string authToken);
        Task<(bool success, string message)> AddSalesOrderDetailsAsync(SalesOrder salesorder, string authToken);
        Task<(bool success, string message)> EditSalesOrderDetailsAsync(SalesOrder salesorder, string authToken);
    }
}
