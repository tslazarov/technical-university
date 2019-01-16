using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Models.Rides
{
    public class CreateRideViewModel
    {
        [Required(ErrorMessage = "RideCreate_FromCityRequired")]
        public string FromCity { get; set; }

        [Required(ErrorMessage = "RideCreate_ToCityRequired")]
        public string ToCity { get; set; }

        [Required(ErrorMessage = "RideCreate_TravelDateRequired")]
        public DateTime? TravelDate { get; set; }

        [Required(ErrorMessage = "RideCreate_FreeSeatsRequired")]
        [RegularExpression("([0-9]{1,2})", ErrorMessage = "RideCreate_FreeSeatsRegular")]
        public int? FreeSeats { get; set; }

        [Required(ErrorMessage = "RideCreate_PriceRequired")]
        [RegularExpression(@"^\d+(\,|\.)?\d{0,2}$", ErrorMessage = "RideCreate_PriceRegular")]
        [Range(0, 9999999999999999.99, ErrorMessage = "RideCreate_PriceRange")]
        public decimal? Price { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
