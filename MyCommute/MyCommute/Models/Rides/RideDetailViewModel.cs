using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Models.Rides
{
    public class RideDetailViewModel
    {
        public Guid Id { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public DateTime? TravelDate { get; set; }
        public int? FreeSeats { get; set; }
        public decimal? Price { get; set; }
        public string AdditionalInformation { get; set; }
        public string DriverName { get; set; }
        public Guid DriverId { get; set; }
        public IEnumerable<RidesUser> RideUsers { get; set; }
        public string CurrentUserId { get; internal set; }
    }
}
