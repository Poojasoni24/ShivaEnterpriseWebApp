using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IModeofPaymentServiceImpl
    {
        Task<List<ModeofPayment>> GetModeofPaymentList(string authToken);
        Task<ModeofPayment> GetModeofPaymentById(string ModeofPaymentId, string authToken);
        Task<(bool successs, string message)> DeleteModeofPayment(string ModeofPaymentId, string authToken);
        Task<(bool success, string message)> AddModeofPaymentDetailsAsync(ModeofPayment ModeofPayment, string authToken);
        Task<(bool success, string message)> EditModeofPaymentDetailsAsync(ModeofPayment ModeofPayment, string authToken);
    }
}
