using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IBrandServiceImpl
    {
        Task<List<Brand>> GetBrandList(string authToken);
        Task<Brand> GetBrandById(Guid BrandId, string authToken);
        Task<(bool successs, string message)> DeleteBrand(Guid BrandId, string authToken);
        Task<(bool success, string message)> AddBrandDetailsAsync(Brand Brand, string authToken);
        Task<(bool success, string message)> EditBrandDetailsAsync(Brand Brand, string authToken);
    }
}

