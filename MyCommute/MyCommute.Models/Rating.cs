using System;
using System.Collections.Generic;

namespace MyCommute.Models
{
    public partial class Rating : IDataItem
    {
        public Guid Id { get; set; }
        public Guid RaterId { get; set; }
        public Guid ReceiverId { get; set; }
        public byte Value { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public RatingType RatingType { get; set; }

        public User Rater { get; set; }
        public User Receiver { get; set; }
    }
}
