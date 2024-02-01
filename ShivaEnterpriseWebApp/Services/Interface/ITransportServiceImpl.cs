using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ITransportServiceImpl
    {
        Task<List<Transport>> GetTransportList(string authToken);
        Task<Transport> GetTransportById(string TransportId, string authToken);
        Task<(bool successs, string message)> DeleteTransport(string TransportId, string authToken);
        Task<(bool success, string message)> AddTransportDetailsAsync(Transport Transport, string authToken);
        Task<(bool success, string message)> EditTransportDetailsAsync(Transport Transport, string authToken);
    }
}
