using ShivaEnterpriseWebApp.DTOs;
using ShivaEnterpriseWebApp.Services.Implementation;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ICompany
    {
        Task<List<CompanyDTO>> GetCompanyDetailsAsync();
        Task<(bool success, string message)> AddCompanyDetailsAsync(CompanyDTO company);
        Task<CompanyDTO> GetCompanyDetailById(Guid companyId);
        Task<(bool successs, string message)> DeleteCompany(Guid companyId);
        Task<(bool success, string message)> EditCompanyDetailsAsync(Guid id, CompanyDTO company);
    }
}
