using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IPurchaseOrderReturnServiceImpl
    {
        Task<(bool success, string message)> AddpurchaseReturnDetailsAsync(PurchaseOrderReturn purchaseReturn, string authToken);

        Task<(bool successs, string message)> DeletepurchaseReturn(Guid purchaseReturnId, string authToken);

        Task<(bool success, string message)> EditpurchaseReturnDetailsAsync(PurchaseOrderReturn purchaseReturn, string authToken);

        Task<PurchaseOrderReturn> GetpurchaseReturnById(Guid purchaseReturnId, string authToken);

        Task<List<PurchaseOrderReturn>> GetpurchaseReturnList(string authToken);
    }
}
