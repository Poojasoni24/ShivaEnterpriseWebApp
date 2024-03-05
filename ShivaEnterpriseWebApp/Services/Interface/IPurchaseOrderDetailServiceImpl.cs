using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IPurchaseOrderDetailServiceImpl
    {
        Task<List<PurchaseOrderDetail>> GetPurchaseOrderDetailList(string authToken);
        Task<PurchaseOrderDetail> GetPurchaseOrderDetailById(string purchaseorderdetailId, string authToken);
        Task<(bool successs, string message)> DeletePurchaseOrderDetail(string purchaseorderdetailId, string authToken);
        Task<(bool success, string message)> AddPurchaseOrderDetailDetailsAsync(PurchaseOrderDetail purchaseorderdetail, string authToken);
        Task<(bool success, string message)> EditPurchaseOrderDetailDetailsAsync(PurchaseOrderDetail purchaseorderdetail, string authToken);
    }
}
