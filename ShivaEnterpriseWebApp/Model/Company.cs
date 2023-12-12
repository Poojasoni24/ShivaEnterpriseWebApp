using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShivaEnterpriseWebApp.Model
{
    [Table("Comapny")]
    public class Company
    {
        [Key]
        public Guid Company_Id { get; set; }

        [Required]
        public string? Company_Code { get; set; }

        [Required]
        public string? Company_Name { get; set; }

        public DateTime? Company_Startyear { get; set; }

        public DateTime? Company_Endyear { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public bool? IsActive { get; set; }
    }
}
