using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IInwardServiceImpl
    {
        Task<List<Inward>> GetInwardList(string authToken);
        Task<Inward> GetInwardById(Guid inwardId, string authToken);
        Task<(bool success, string message)> DeleteInward(Guid inwardId, string authToken);
        Task<(bool success, string message)> AddInwardDetailsAsync(Inward inward, string authToken);
        Task<Product> GetProductByPurchseOrderId(Guid productId, string authToken);
        Task<(bool success, string message)> EditInwardDetailsAsync(Inward inward, string authToken);
    }
}
