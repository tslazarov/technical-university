using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Extensions.Hangfire
{
    public class FuelPriceModel
    {
        public string Status { get; set; }
        public string Fuel { get; set; }
        public decimal Price { get; set; }
        public string Date { get; set; }
    }
}
