using System.ComponentModel.DataAnnotations;

namespace MyCommute.Models.Auth
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "SignIn_EmailRequired")]
        public string Email { get; set; }

        [Required(ErrorMessage = "SignIn_PasswordRequired")]
        public string Password { get; set; }
    }
}
