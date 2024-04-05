namespace FBLA_project
{
    public class UnprotectedData
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Gender { get; set; }
        public required string Name { get; set; }
        public List<Membership>? Memberships { get; set; }
    }
}
