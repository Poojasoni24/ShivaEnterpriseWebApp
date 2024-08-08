using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IStockServiceImpl
    {
        Task<List<Stock>> GetStockList(string authToken);
        Task<(bool success, string value)> AddEditStockDetailsAsync(List<Stock> stock, string authToken);
        Task<Stock> GetStockByProductId(Guid productId, string authToken);

    }
}
