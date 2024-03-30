using System.Security.Policy;

namespace FBLA_project
{
    public class User : UserBase
    {
        public required int Id { get; set; }
        public bool IsAdmin { get; set; }
    }
}