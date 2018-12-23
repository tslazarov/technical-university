using System;
using System.Collections.Generic;

namespace MyCommute.Models
{
    public partial class Ride : IDataItem
    {
        public Ride()
        {
            RidesUsers = new HashSet<RidesUser>();
        }

        public Guid Id { get; set; }
        public Guid? DriverId { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public DateTime? TravelDate { get; set; }
        public int? FreePlaces { get; set; }
        public decimal? Price { get; set; }

        public ICollection<RidesUser> RidesUsers { get; set; }
    }
}
