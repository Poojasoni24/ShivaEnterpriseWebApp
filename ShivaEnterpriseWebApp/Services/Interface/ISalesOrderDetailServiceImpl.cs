using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ISalesOrderDetailServiceImpl
    {
        Task<List<SalesOrderDetail>> GetSalesOrderDetailList(string authToken);
        Task<SalesOrderDetail> GetSalesOrderDetailById(Guid salesorderdetailId, string authToken);
        Task<(bool successs, string message)> DeleteSalesOrderDetail(Guid salesorderdetailId, string authToken);
        Task<(bool success, string message)> AddSalesOrderDetailDetailsAsync(SalesOrderDetail salesorderdetail, string authToken);
        Task<(bool success, string message)> EditSalesOrderDetailDetailsAsync(SalesOrderDetail salesorderdetail, string authToken);
    }
}
