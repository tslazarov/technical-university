using System.ComponentModel.DataAnnotations;

namespace MyCommute.Models.Auth
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Have to supply a username")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Have to supply a password")]
        public string Password { get; set; }
    }
}
