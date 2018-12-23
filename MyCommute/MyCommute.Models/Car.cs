using System;
using System.Collections.Generic;

namespace MyCommute.Models
{
    public partial class Car : IDataItem
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int? Seats { get; set; }
        public int? FuelType { get; set; }

        public User Owner { get; set; }
    }
}
