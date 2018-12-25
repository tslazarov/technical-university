using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Models.Auth
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Profile_FirstNameRequired")]
        [RegularExpression(@"^[a-zA-Z ,.'-]+$", ErrorMessage = "Profile_FirstNameRegular")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Profile_LastNameRequired")]
        [RegularExpression(@"^[a-zA-Z ,.'-]+$", ErrorMessage = "Profile_LastNameRegular")]
        public string LastName { get; set; }

        [RegularExpression("([0-9]{10})|^$", ErrorMessage = "Profile_PhoneNumberRegular")]
        public string PhoneNumber { get; set; }

        public string Provider { get; set; }
    }
}
