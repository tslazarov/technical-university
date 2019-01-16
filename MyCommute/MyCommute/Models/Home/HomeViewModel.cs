using System;
using System.Collections.Generic;

namespace MyCommute.Models.Home
{
    public class HomeViewModel
    {
        public IEnumerable<RideSummary> Rides { get; set; }
    }

    public class RideSummary
    {
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public Guid Id { get; set; }
    }
}