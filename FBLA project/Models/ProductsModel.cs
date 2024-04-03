using System.Security.Cryptography.X509Certificates;

namespace FBLA_project
{
    public class ProductsModel : BaseModel
    {
        public Membership? Membership { get; set; }
        public bool LoginRequired { get; set; }
        public bool PurchaseSuccessful { get; set; }
        public ProductsModel() {
            base.UnprotectedData = null;
            LoginRequired = false;
            PurchaseSuccessful = false;
        }

    }
}
