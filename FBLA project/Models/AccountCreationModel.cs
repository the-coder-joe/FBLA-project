using System.ComponentModel.DataAnnotations;

namespace FBLA_project
{
    public class AccountCreationModel : BaseModel
    {

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public required string ConfirmPassword { get; set; }
        public required UnprotectedData UnprotectedInfo { get; set; }

        public string? Message = null;



    }
}
