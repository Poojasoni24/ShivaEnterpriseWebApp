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
        public string Company_Code { get; set; }

        [DisplayName("Company Name")]
        public string Company_Name { get; set; }

        [DisplayName("Company Start Date")]
        public DateTime? Company_Startyear { get; set; }

        [DisplayName("Company End Date")]
        public DateTime? Company_Endyear { get; set; }

        [DisplayName("Status")]
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
