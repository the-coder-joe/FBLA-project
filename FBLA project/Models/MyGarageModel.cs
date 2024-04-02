namespace FBLA_project { 
    public class MyGarageModel : BaseModel
    {
        public string Name { get { return base.UnprotectedData.Name; } }
    }
}
