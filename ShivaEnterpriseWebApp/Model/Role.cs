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

        public DateTime CreatedDateTime { get; set; }
    }
}
