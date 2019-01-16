using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Models.Fuel
{
    public class CalculateViewModel
    {
        public string FuelType { get; set; }
        public decimal Distance { get; set; }
        public decimal Consumption { get; set; }
        public decimal Seats { get; set; }

        public decimal FuelPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal PricePerPerson { get; set; }
    }
}
