using System.ComponentModel.DataAnnotations;

namespace FBLA_project
{
    public class Car
    {
        [Required]
        [Display(Name = "Make")]
        public required string Make { get; set; }
        [Required]
        [Display(Name = "Model")]
        public required string Model { get; set; }
        [Required]
        [Display(Name = "Color")]
        public required string Color { get; set; }
         [Required]
        [Display(Name = "LicensePlate")]
        public required string LicensePlate { get; set; }
    }
}
