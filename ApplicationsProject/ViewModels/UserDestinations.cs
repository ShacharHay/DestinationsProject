using System.ComponentModel;

namespace ApplicationsProject.ViewModels
{
    public class UserDestinations
    {
        public int Id { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Destination")]
        public string Title { get; set; }
    }
}