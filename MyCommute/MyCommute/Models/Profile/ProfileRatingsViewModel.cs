using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Models.Profile
{
    public class ProfileRatingsViewModel
    {
        public IList<RatingViewModel> Ratings { get; set; }
        public string FilterBy { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }

    public class RatingViewModel
    {
        public Guid RatingId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; }
        public RatingType RatingType { get; set; }
    }
}
