using System.ComponentModel;

namespace ApplicationsProject.ViewModels
{
    public class DestinationComment
    {
        public int Id { get; set; }

        [DisplayName("Destination")]
        public string Title { get; set; }

        [DisplayName("Number Of Comments")]
        public int NumberOfComment { get; set; }

        [DisplayName("Author")]
        public string AuthorFullName { get; set; }
    }
}