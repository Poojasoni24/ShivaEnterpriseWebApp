using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IProductTypeServiceImpl
    {
        Task<List<ProductType>> GetProductTypeList(string authToken);
        Task<ProductType> GetProductTypeById(string productTypeId, string authToken);
        Task<(bool successs, string message)> DeleteProductType(string productTypeId, string authToken);
        Task<(bool success, string message)> AddProductTypeDetailsAsync(ProductType productType, string authToken);
        Task<(bool success, string message)> EditProductTypeDetailsAsync(ProductType productType, string authToken);
    }
}
