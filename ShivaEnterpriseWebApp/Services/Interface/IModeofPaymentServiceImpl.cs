using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IModeofPaymentServiceImpl
    {
        Task<List<ModeofPayment>> GetModeofPaymentList(string authToken);
        Task<ModeofPayment> GetModeofPaymentById(Guid ModeofPaymentId, string authToken);
        Task<(bool successs, string message)> DeleteModeofPaymentAsync(Guid ModeofPaymentId, string authToken);
        Task<(bool success, string message)> AddModeofPaymentAsync(ModeofPayment ModeofPayment, string authToken);
        Task<(bool success, string message)> EditModeofPaymentAsync(ModeofPayment ModeofPayment, string authToken);
    }
}
