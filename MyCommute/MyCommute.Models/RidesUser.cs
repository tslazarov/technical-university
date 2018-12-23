using System;
using System.Collections.Generic;

namespace MyCommute.Models
{
    public partial class RidesUser
    {
        public Guid RideId { get; set; }
        public Guid PassengerId { get; set; }

        public User Passenger { get; set; }
        public Ride Ride { get; set; }
    }
}
