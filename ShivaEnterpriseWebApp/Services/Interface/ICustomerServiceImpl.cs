using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ICustomerServiceImpl
    {
        Task<List<Customer>> GetCustomerList(string authToken);
        Task<Customer> GetCustomerById(Guid customerId, string authToken);
        Task<(bool successs, string message)> DeleteCustomer(Guid customerId, string authToken);
        Task<(bool success, string message)> AddCustomerDetailsAsync(Customer customer, string authToken);
        Task<(bool success, string message)> EditCustomerDetailsAsync(Customer customer, string authToken);
        Task GetCustomerById(string? authToken);
    }
}
