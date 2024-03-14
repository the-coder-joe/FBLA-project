using System.ComponentModel.DataAnnotations;

namespace FBLA_project
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }
    }
}