using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShivaEnterpriseWebApp.Model
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerType { get; set; }
        public string? CustomerAddress { get; set; }
        public string Phoneno { get; set; }
        public string Email { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public Guid cityId { get; set; }
        public string? Remark { get; set; }
        public bool IsActive { get; set; }
        public decimal? CustomerDiscount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public List<SelectListItem> CityList { get; set; }
        public City City { get; set; }

    }
}
