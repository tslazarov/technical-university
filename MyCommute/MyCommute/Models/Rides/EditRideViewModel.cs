using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Models.Rides
{
    public class EditRideViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "RideCreateEdit_FromCityRequired")]
        public string FromCity { get; set; }

        [Required(ErrorMessage = "RideCreateEdit_ToCityRequired")]
        public string ToCity { get; set; }

        [Required(ErrorMessage = "RideCreateEdit_TravelDateRequired")]
        public DateTime? TravelDate { get; set; }

        [Required(ErrorMessage = "RideCreateEdit_FreeSeatsRequired")]
        [RegularExpression("([0-9]{1,2})", ErrorMessage = "RideCreateEdit_FreeSeatsRegular")]
        public int? FreeSeats { get; set; }

        [Required(ErrorMessage = "RideCreateEdit_PriceRequired")]
        [RegularExpression(@"^\d+(\,|\.)?\d{0,2}$", ErrorMessage = "RideCreateEdit_PriceRegular")]
        [Range(0, 9999999999999999.99, ErrorMessage = "RideCreateEdit_PriceRange")]
        public decimal? Price { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
