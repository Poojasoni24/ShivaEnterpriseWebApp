using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IProductGroupServiceImpl
    {
        Task<List<ProductGroup>> GetProductGroupList(string authToken);
        Task<ProductGroup> GetProductGroupById(string productGroupId, string authToken);
        Task<(bool successs, string message)> DeleteProductGroup(string productGroupId, string authToken);
        Task<(bool success, string message)> AddProductGroupDetailsAsync(ProductGroup productGroup, string authToken);
        Task<(bool success, string message)> EditProductGroupDetailsAsync(ProductGroup productGroup, string authToken);
    }
}
