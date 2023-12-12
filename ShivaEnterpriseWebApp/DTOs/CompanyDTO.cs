using Newtonsoft.Json;

namespace ShivaEnterpriseWebApp.DTOs
{
    public class CompanyDTO
    {
        public Guid Company_Id { get; set; }

        public string? Company_Code { get; set; }

        public string? Company_Name { get; set; }

        public DateTime? Company_Startyear { get; set; }

        public DateTime? Company_Endyear { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public bool? IsActive { get; set; }


    }
}
