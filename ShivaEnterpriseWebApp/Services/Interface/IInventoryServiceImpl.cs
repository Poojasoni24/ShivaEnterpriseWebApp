using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IInventoryServiceImpl
    {
        Task<List<Inventory>> GetInventoryList(string authToken);
        Task<(bool success, string value)> AddEditInventoryDetailsAsync(List<Inventory> lst, string authToken);
    }
}
