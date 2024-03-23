using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IPurchaseOrderServiceImpl
    {
        Task<List<PurchaseOrder>> GetPurchaseOrderList(string authToken);
        Task<PurchaseOrder> GetPurchaseOrderById(string purchaseorderId, string authToken);
        Task<(bool successs, string message)> DeletePurchaseOrder(string purchaseorderId, string authToken);
        Task<(bool success, string value)> AddPurchaseOrderDetailsAsync(PurchaseOrder purchaseorder, string authToken);
        Task<(bool success, string message)> EditPurchaseOrderDetailsAsync(PurchaseOrder purchaseorder, string authToken);
    }
}
