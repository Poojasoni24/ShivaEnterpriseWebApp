using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ISalesOrderServiceImpl
    {
        Task<List<SalesOrder>> GetSalesOrderList(string authToken);
        Task<SalesOrder> GetSalesOrderById(Guid salesorderId, string authToken);
        Task<(bool successs, string message)> DeleteSalesOrder(string salesorderId, string authToken);
        Task<(bool success, string value)> AddSalesOrderDetailsAsync(SalesOrder salesorder, string authToken);
        Task<(bool success, string value)> EditSalesOrderDetailsAsync(SalesOrder salesorder, string authToken);
    }
}
