using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IOutwardServiceImpl
    {
        Task<List<Outward>> GetOutwardList(string authToken);
        Task<Outward> GetOutwardById(Guid outwardsId, string authToken);
        Task<(bool successs, string message)> DeleteOutward(string outwardsId, string authToken);
        Task<(bool success, string message)> AddOutwardDetailsAsync(Outward outwards, string authToken);
        Task<(bool success, string message)> EditOutwardDetailsAsync(Outward outwards, string authToken);
        Task<List<Customer>> getCustomerFromSaleOrderId(Guid saleOrderId, string authToken);
    }
}
