using System.ComponentModel.DataAnnotations;

namespace FBLA_project
{
    public class ApplicationModel
    { 
        public required Job Job { get; set; }

        
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
