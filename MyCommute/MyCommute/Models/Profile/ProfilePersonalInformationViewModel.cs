using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Models.Profile
{
    public class ProfilePersonalInformationViewModel
    {
        [Required(ErrorMessage = "Profile_FirstNameRequired")]
        [RegularExpression(@"^[a-zA-Z ,.'-]+$", ErrorMessage = "Profile_FirstNameRegular")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Profile_LastNameRequired")]
        [RegularExpression(@"^[a-zA-Z ,.'-]+$", ErrorMessage = "Profile_LastNameRegular")]
        public string LastName { get; set; }
    }
}
