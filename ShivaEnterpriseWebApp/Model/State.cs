using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace ShivaEnterpriseWebApp.Model
{
    public class State
    {
        public string State_Id { get; set; }

        [DisplayName("State Name")]
        public string State_Name { get; set; }

        [DisplayName("State Code")]
        public string State_Code {  get; set; }
        public string Country_Id { get; set; }

        [DisplayName("State Status")]
        public bool IsActive { get; set; }
        public List<SelectListItem> CountryList { get; set; }
        public Country? country { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
