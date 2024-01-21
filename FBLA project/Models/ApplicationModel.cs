namespace FBLA_project
{
    public class ApplicationModel
    {
        public required Job Job { get; set; }
        public ApplicationFields? Application { get; set; }
        public IFormFile? ResumeFile { get; set; }
        public string? Message { get; set; }
        public bool Completed { get; set; }
    }
}
