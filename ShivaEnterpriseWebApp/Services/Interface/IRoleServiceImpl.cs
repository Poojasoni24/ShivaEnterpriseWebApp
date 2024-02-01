using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IRoleServiceImpl 
    {
        Task<List<Role>> GetRoleList(string authToken);
        Task<Role> GetRoleById(string roleId, string authToken);
        Task<(bool successs, string message)> DeleteRole(string roleId, string authToken);
        Task<(bool success, string message)> AddRoleAsync(Role role, string authToken);
        Task<(bool success, string message)> EditRoleAsync(Role role, string authToken);
    }
}
