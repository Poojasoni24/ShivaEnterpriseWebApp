using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IIssueServiceImpl
    {
        Task<List<Issue>> GetIssueList(string authToken);
        Task<Issue> GetIssueById(string IssueId, string authToken);
        Task<(bool successs, string message)> DeleteIssue(string IssueId, string authToken);
        Task<(bool success, string message)> AddIssueDetailsAsync(Issue Issue, string authToken);
        Task<(bool success, string message)> EditIssueDetailsAsync(Issue Issue, string authToken);
    }
}

