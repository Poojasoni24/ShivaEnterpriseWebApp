using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace ShivaEnterpriseWebApp.Model
{
    public class City
    {
        public string cityId { get; set; }

        [DisplayName("City Code")]
        public string City_Code { get; set; }

        [DisplayName("City Name")]
        public string City_Name { get; set; }

        public string State_Id { get; set; }

        public bool IsActive { get; set; }
        public List<SelectList> StateList { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public State State { get; set; }

        public Country Country { get; set; }
    }
}
