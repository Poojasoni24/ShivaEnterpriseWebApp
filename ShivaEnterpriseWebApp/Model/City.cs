using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace ShivaEnterpriseWebApp.Model
{
    public class City
    {
        public string City_Id { get; set; }

        [DisplayName("City Name")]
        public string City_Name { get; set; }        
        public string State_Id { get; set; }

        public List<SelectList> StateList { get; set; }
        public DateTime CreatedDateAndTime { get; set; }
        public DateTime? UpdatedDateAndTime { get; set; }
    }
}
