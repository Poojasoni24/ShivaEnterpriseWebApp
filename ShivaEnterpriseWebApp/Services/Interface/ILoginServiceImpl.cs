using ShivaEnterpriseWebApp.DTOs;
using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ILoginServiceImpl
    {
        Task<UserDetails> GetUserdetail(string userName);
        Task<AuthDAOs> PerformLogin(LoginModel loginModel);
        Task<string> PerformLogout();
    }
}
