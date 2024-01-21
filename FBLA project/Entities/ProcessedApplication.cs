namespace FBLA_project
{
    public class ProcessedApplication
    {
        public required string ApplicationId { get; set; }
        public string? ResumeFileName { get; set; }
        public ApplicationFields? Fields { get; set; }
        public required Job Job { get; set; }
    }
}
