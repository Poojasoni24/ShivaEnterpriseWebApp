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
        public string State_Code {  get; set; }
        public string Country_Id { get; set; }

        public List<SelectListItem> CountryList { get; set; }
        public DateTime CreatedDateAndTime { get; set; }
        public DateTime? UpdatedDateAndTime { get; set; }
    }
}
