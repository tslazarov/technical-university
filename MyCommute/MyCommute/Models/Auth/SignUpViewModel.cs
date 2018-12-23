using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyCommute.Models.Auth
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Have to supply an email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Have to supply a password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Have to supply a confirm password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
