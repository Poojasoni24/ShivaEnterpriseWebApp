using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface IVendorServiceImpl
    {
        Task<List<Vendor>> GetVendorList(string authToken);
        Task<Vendor> GetVendorById(string vendorId, string authToken);
        Task<(bool successs, string message)> DeleteVendor(string vendorId, string authToken);
        Task<(bool success, string message)> AddVendorDetailsAsync(Vendor vendor, string authToken);
        Task<(bool success, string message)> EditVendorDetailsAsync(Vendor vendor, string authToken);
    }
}
