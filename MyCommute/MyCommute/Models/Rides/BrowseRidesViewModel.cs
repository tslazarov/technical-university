using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Models.Rides
{
    public class BrowseRidesViewModel
    {
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string OrderBy { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<Ride> Rides { get; set; }
    }
}
