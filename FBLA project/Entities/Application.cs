namespace FBLA_project
{
    public class Application
    {
        public Job? Job { get; set; }
        public required string ApplicationId { get; set; }
        public string? Name { get; set; }
        public string? ResumeFileName { get; set; }
        public string? PhoneNumber { get; set; }
        //Yapper BS Questions
        public string? Strengths { get; set; }
        public string? WhyThisJob { get; set; }
    }
}
