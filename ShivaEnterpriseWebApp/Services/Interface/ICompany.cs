using ShivaEnterpriseWebApp.DTOs;
using ShivaEnterpriseWebApp.Model;
using ShivaEnterpriseWebApp.Services.Implementation;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ICompany
    {
        Task<List<Company>> GetCompanyDetailsAsync(string authToken);
        Task<(bool success, string message)> AddCompanyDetailsAsync(Company company, string authToken);
        Task<Company> GetCompanyDetailById(Guid companyId, string authToken);
        Task<(bool successs, string message)> DeleteCompany(Guid companyId, string authToken);
        Task<(bool success, string message)> EditCompanyDetailsAsync(Company company, string authToken);
    }
}
