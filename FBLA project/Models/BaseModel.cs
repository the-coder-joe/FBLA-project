namespace FBLA_project
{
    public class BaseModel
    {
        public UnprotectedData? UnprotectedData { get; set; } 
        public bool LoggedIn { get { return UnprotectedData != null; } }
    }
}
