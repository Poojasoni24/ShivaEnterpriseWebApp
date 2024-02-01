using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShivaEnterpriseWebApp.Model
{
    public class Country
    {
        public string Country_Id { get; set; }

        [DisplayName("Country Name")]
        public string Country_Name { get; set; }

        [DisplayName("Country Code")]
        public string Country_Code { get; set; }
        public DateTime CreatedDateAndTime { get; set; }
        public DateTime UpdatedDateAndTime { get; set; }

    }
}
