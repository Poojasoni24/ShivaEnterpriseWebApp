using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IPurchaseOrderDetailServiceImpl
    {
        Task<List<PurchaseOrderDetail>> GetPurchaseOrderDetailList(string authToken);
        Task<PurchaseOrderDetail> GetPurchaseOrderDetailById(string purchaseorderdetailId, string authToken);
        Task<(bool successs, string message)> DeletePurchaseOrderDetail(string purchaseorderdetailId, string authToken);
        Task<(bool success, string message)> AddPurchaseOrderDetailDetailsAsync(List<PurchaseOrderDetail> purchaseorderdetail, string authToken);
        Task<(bool success, string message)> EditPurchaseOrderDetailDetailsAsync(List<PurchaseOrderDetail> purchaseorderdetail, string authToken);
    }
}
