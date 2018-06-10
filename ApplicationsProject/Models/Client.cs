using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApplicationsProject.Models
{
    public class Client
    {
        public Gender Gender { get; set; }

        public int ClientId { get; set; }

        [Required]
        [DisplayName("Username")]
        public string ClientName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public string Password { get; set; }

        [DisplayName("Administrator")]
        public bool IsAdmin { get; set; }
        public virtual List<Comment> Comments { get; set; }

        public List<Destination> Destinations { get; set; } = new List<Destination>();
    } 
}