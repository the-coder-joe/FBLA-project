﻿namespace FBLA_project { 
    public class MyGarageModel : BaseModel
    {
        public string? Name { get { return UnprotectedData?.Name; } }
        public List<Membership>? Memberships { get { return UnprotectedData?.Memberships;  } }
    }
}
