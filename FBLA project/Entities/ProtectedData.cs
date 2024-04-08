namespace FBLA_project
{
    public class ProtectedData
    {
        public required string PasswordHash { get; set; }
        public required string Salt { get; set; }
        public required bool IsAdmin { get; set; }
        
    }
}
