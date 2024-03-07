namespace FBLA_project
{
    public class ProcessedApplication
    {
        public string ApplicationId { get; set; }
        public string? ResumeFileName { get; set; }
        public ApplicationFields? Fields { get; set; }
        public Job Job { get; set; }
    }
}