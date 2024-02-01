using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ICityServiceImpl
    {
        Task<List<City>> GetCityList(string authToken);
        Task<City> GetCityById(string CityId, string authToken);
        Task<(bool successs, string message)> DeleteCity(string cityId, string authToken);
        Task<(bool success, string message)> AddCityDetailsAsync(City city, string authToken);
        Task<(bool success, string message)> EditCityDetailsAsync(City city, string authToken);
    }
}
