using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShivaEnterpriseWebApp.Model
{
    public class Role
    {
        public string Id { get; set; }

        [DisplayName("Role Name")]
        public string Name { get; set; }

        public bool IsActive { get; set; }
        public string? NormalizedName { get; set; }

        public string? ConcurrencyStamp { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
