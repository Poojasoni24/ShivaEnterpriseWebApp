using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IUnitServiceImpl
    {
        Task<List<Unit>> GetUnitList(string authToken);
        Task<Unit> GetUnitById(string UnitId, string authToken);
        Task<(bool successs, string message)> DeleteUnit(Guid UnitId, string authToken);
        Task<(bool success, string message)> AddUnitDetailsAsync(Unit Unit, string authToken);
        Task<(bool success, string message)> EditUnitDetailsAsync(Unit Unit, string authToken);
    }
}
