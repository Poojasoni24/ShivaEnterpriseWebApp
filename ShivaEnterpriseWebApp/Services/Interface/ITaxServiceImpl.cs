﻿using ShivaEnterpriseWebApp.Model;

namespace ShivaEnterpriseWebApp.Services.Interface
{
    public interface ITaxServiceImpl
    {
        Task<List<Tax>> GetTaxList(string authToken);
        Task<Tax> GetTaxById(string TaxId, string authToken);
        Task<(bool successs, string message)> DeleteTax(string TaxId, string authToken);
        Task<(bool success, string message)> AddTaxDetailsAsync(Tax Tax, string authToken);
        Task<(bool success, string message)> EditTaxDetailsAsync(Tax Tax, string authToken);
    }
}
