using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IBranchServiceImpl
    {
        Task<List<Branch>> GetBranchList(string authToken);
        Task<Branch> GetBranchById(Guid branchId, string authToken);
        Task<(bool successs, string message)> DeleteBranch(string branchId, string authToken);
        Task<(bool success, string message)> AddBranchDetailsAsync(Branch branch, string authToken);
        Task<(bool success, string message)> EditBranchDetailsAsync(Branch branch, string authToken);
    }
}
