using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Models.Profile
{
    public class ProfileCarInformationViewModel
    {
        [Required(ErrorMessage = "Profile_CarBrandRequired")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Profile_CarBrandRegular")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Profile_CarModelRequired")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Profile_CarModelRegular")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Profile_CarSeatsRequired")]
        [RegularExpression("([0-9]{1,2})", ErrorMessage = "Profile_CarSeatsRegular")]
        public int? Seats { get; set; }

        public string FuelType { get; set; }
    }
}
