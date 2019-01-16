using System;
using System.Collections.Generic;

namespace MyCommute.Models
{
    public partial class RidesUser : IDataItem
    {
        public Guid RideId { get; set; }
        public Guid UserId { get; set; }

        public Ride Ride { get; set; }
        public User User { get; set; }
    }
}
