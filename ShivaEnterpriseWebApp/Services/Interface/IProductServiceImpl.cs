using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IProductServiceImpl
    {
        Task<List<Product>> GetProductList(string authToken);
        Task<Product> GetProductById(Guid productId, string authToken);
        Task<(bool successs, string message)> DeleteProduct(string productId, string authToken);
        Task<(bool success, string message)> AddProductDetailsAsync(Product product, string authToken);
        Task<(bool success, string message)> EditProductDetailsAsync(Product product, string authToken);
    }
}
