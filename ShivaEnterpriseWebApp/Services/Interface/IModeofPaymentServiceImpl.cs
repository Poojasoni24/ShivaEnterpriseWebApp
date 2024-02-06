using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IModeofPaymentServiceImpl
    {
        Task<List<ModeofPayment>> GetModeofPaymentList(string authToken);
        Task<ModeofPayment> GetModeofPaymentById(string ModeofPaymentId, string authToken);
        Task<(bool successs, string message)> DeleteModeofPayment(string ModeofPaymentId, string authToken);
        Task<(bool success, string message)> AddModeofPaymentAsync(ModeofPayment ModeofPayment, string authToken);
        Task<(bool success, string message)> EditModeofPaymentAsync(ModeofPayment ModeofPayment, string authToken);
    }
}
