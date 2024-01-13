using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IUserServicecImpl
    {
        Task<List<UserDetails>> GetUserList(string authToken);
        Task<UserDetails> GetUserById(string userId, string authToken);
        Task<(bool successs, string message)> DeleteUser(string userId, string authToken);
        Task<(bool success, string message)> AddUserDetailsAsync(UserRegistration userDetails, string authToken);
        Task<(bool success, string message)> EditUserDetailsAsync(UserRegistration userDetails, string authToken);
    }
}
