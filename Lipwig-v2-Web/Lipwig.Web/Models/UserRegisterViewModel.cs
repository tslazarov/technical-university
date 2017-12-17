using Lipwig.Models;

namespace Lipwig.Web.Models
{
    public class UserRegisterViewModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public decimal Balance { get; set; }

        public Currency Currency { get; set; }
    }
}