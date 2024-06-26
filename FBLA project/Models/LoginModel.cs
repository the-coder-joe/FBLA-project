﻿using System.ComponentModel.DataAnnotations;

namespace FBLA_project
{
    public class LoginModel : BaseModel
    {
        [Required]
        [Display(Name = "Username")]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        public string? Message = null;
    }
}