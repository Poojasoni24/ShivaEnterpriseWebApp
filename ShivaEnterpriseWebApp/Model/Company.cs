using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShivaEnterpriseWebApp.Model
{
    [Table("Comapny")]
    public class Company
    {
        public Guid Company_Id { get; set; }

        [DisplayName("Company Code")]
        public string? Company_Code { get; set; }

        [DisplayName("Company Name")]
        public string? Company_Name { get; set; }

        [DisplayName("Company Start Date")]
        public DateTime? Company_Startyear { get; set; }

        [DisplayName("Company End Date")]
        public DateTime? Company_Endyear { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        [DisplayName("Status")]
        public bool IsActive { get; set; }
    }
}
