using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ICountryServiceImpl
    {
        Task<List<Country>> GetCountryList(string authToken);
        Task<Country> GetCountryById(string countryId, string authToken);
        Task<(bool successs, string message)> DeleteCountry(string countryId, string authToken);
        Task<(bool success, string message)> AddCountryDetailsAsync(Country country, string authToken);
        Task<(bool success, string message)> EditCountryDetailsAsync(Country country, string authToken);
    }
}
