using Telerik.Charting;

namespace Lipwig.Desktop.Models
{
    public class PieChartData : DataPoint
    {
        public decimal Value { get; set; }

        public string Category { get; set; }
    }
}
