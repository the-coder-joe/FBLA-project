using System.Security.Policy;

namespace FBLA_project
{
    public class User
    { 
        public required string Id { get; set; }
        public required ProtectedData ProtectedInfo { get; set; }
        public required UnprotectedData UnprotectedInfo { get; set; }
    }
}