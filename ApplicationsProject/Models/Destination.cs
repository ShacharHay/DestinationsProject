using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApplicationsProject.Models
{
    public class Destination
    {
        //[Required]
        [ForeignKey("Client")]
        [DisplayName("Posting Client")]
        public int? ClientId { get; set; }

        //[Required]
        [ForeignKey("Continent")]
        [DisplayName("Continent ID")]
        public int? ContinentId { get; set; }

        public int DestinationId { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Created at")]
        public DateTime CreationDate { get; set; }

        public virtual Client Client { get; set; }

        public virtual Continent Continent { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}