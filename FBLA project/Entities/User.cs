using System.Security.Policy;

namespace FBLA_project
{
    public class User
    {

        public required string Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public string Username { get; set; }
        public string? Password { get; set; }


    }
}