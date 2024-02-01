using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IStateServiceImpl
    {
        Task<List<State>> GetStateList(string authToken);
        Task<State> GetStateById(string stateId, string authToken);
        Task<(bool successs, string message)> DeleteState(string stateId, string authToken);
        Task<(bool success, string message)> AddStateDetailsAsync(State state, string authToken);
        Task<(bool success, string message)> EditStateDetailsAsync(State state, string authToken);
    }
}
