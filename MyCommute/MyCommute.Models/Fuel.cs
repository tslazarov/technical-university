using System;
using System.Collections.Generic;

namespace MyCommute.Models
{
    public partial class Fuel : IDataItem
    {
        public string FuelType { get; set; }
        public decimal FuelPrice { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid Id { get; set; }
    }
}
