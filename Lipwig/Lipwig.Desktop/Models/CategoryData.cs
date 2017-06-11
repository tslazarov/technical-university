using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Charting;

namespace Lipwig.Desktop.Models
{
    public class CategoryData : DataPoint
    {
        public decimal Value { get; set; }

        public string Category { get; set; }
    }
}
