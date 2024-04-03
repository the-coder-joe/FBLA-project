namespace FBLA_project { 
    public class MyGarageModel : BaseModel
    {
        public string? Name { get { return UnprotectedData?.Name; } }
        public Membership? Membership { get { return UnprotectedData?.Membership;  } }
    }
}
