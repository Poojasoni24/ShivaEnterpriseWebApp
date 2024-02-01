using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IProductCategoryServiceImpl
    {
        Task<List<ProductCategory>> GetProductCategoryList(string authToken);
        Task<ProductCategory> GetProductCategoryById(string productCategoryId, string authToken);
        Task<(bool successs, string message)> DeleteProductCategory(string productCategoryId, string authToken);
        Task<(bool success, string message)> AddProductCategoryDetailsAsync(ProductCategory productCategory, string authToken);
        Task<(bool success, string message)> EditProductCategoryDetailsAsync(ProductCategory productCategory, string authToken);
    }
}
