using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApplicationsProject.Models
{
    public class Continent
    {
        public int ContinentId { get; set; }
        [Required]
        [DisplayName("Continent Name")]
        public string Name { get; set; }
        public virtual List<Destination> Destinations { get; set; }
    }
}