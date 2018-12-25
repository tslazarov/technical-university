using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyCommute.Models.Auth
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "SignUp_EmailRequired")]
        [RegularExpression(@"^([\w\-\.]+)@((\[([0-9]{1,3}\.){3}[0-9]{1,3}\])|(([\w\-]+\.)+)([a-zA-Z]{2,4}))$", ErrorMessage = "SignUp_EmailRegular")]
        public string Email { get; set; }

        [Required(ErrorMessage = "SignUp_PasswordRequired")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "SignUp_PasswordLength")]
        public string Password { get; set; }

        [Required(ErrorMessage = "SignUp_ConfirmPasswordRequired")]
        [Compare("Password", ErrorMessage = "SignUp_ConfirmPasswordMatch")]
        public string ConfirmPassword { get; set; }
    }
}
