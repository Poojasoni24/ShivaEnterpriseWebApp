using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ISalesReturnDetailServiceImpl
    {
        Task<List<SalesReturnDetail>> GetSalesReturnDetailList(string authToken);
        Task<SalesReturnDetail> GetSalesReturnDetailById(Guid SalesReturndetailId, string authToken);
        Task<(bool successs, string message)> DeleteSalesReturnDetail(Guid SalesReturndetailId, string authToken);
        Task<(bool success, string message)> AddSalesReturnDetailDetailsAsync(List<SalesReturnDetail> SalesReturndetail, string authToken);
        Task<(bool success, string message)> EditSalesReturnDetailDetailsAsync(SalesReturnDetail SalesReturndetail, string authToken);
    }
}
