using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IPurchaseOrderReturnServiceImpl
    {
        Task<(bool success, string message)> AddpurchaseReturnDetailsAsync(purchaseOrderReturn purchaseReturn, string authToken);

        Task<(bool successs, string message)> DeletepurchaseReturn(Guid purchaseReturnId, string authToken);


        Task<(bool success, string message)> EditpurchaseReturnDetailsAsync(purchaseOrderReturn purchaseReturn, string authToken);

        Task<purchaseOrderReturn> GetpurchaseReturnById(Guid purchaseReturnId, string authToken);

        Task<List<purchaseOrderReturn>> GetpurchaseReturnList(string authToken);
    }
}
