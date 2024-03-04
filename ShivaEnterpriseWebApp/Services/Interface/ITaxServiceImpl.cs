using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ITaxServiceImpl
    {
        Task<List<Tax>> GetTaxList(string authToken);
        Task<Tax> GetTaxById(Guid TaxId, string authToken);
        Task<(bool successs, string message)> DeleteTax(Guid TaxId, string authToken);
        Task<(bool success, string message)> AddTaxDetailsAsync(Tax tax, string authToken);
        Task<(bool success, string message)> EditTaxDetailsAsync(Tax tax, string authToken);
    }
}
