using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ISalesmanAgentServiceImpl
    {
        Task<List<SalesmanAgent>> GetSalesmanAgentList(string authToken);
        Task<SalesmanAgent> GetSalesmanAgentById(string salesmanAgentId, string authToken);
        Task<(bool successs, string message)> DeleteSalesmanAgent(string salesmanAgentId, string authToken);
        Task<(bool success, string message)> AddSalesmanAgentDetailsAsync(SalesmanAgent salesmanAgent, string authToken);
        Task<(bool success, string message)> EditSalesmanAgentDetailsAsync(SalesmanAgent salesmanAgent, string authToken);
    }
}
