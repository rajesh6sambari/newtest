using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingTutor.UI.DataVisuals
{
    public class ThresholdMultiLineChart
    {
        public string Id { get; set; }
        public IList<ThresholdMultiLineChartLine> Lines { get; set; } = new List<ThresholdMultiLineChartLine>();
        public ThresholdMultiLineChartLine ThresholdLine { get; set; }
        public ChartPoint Maximum { get; set; }
        public ChartPoint Minimum { get; set; }
        public string XAxis { get; set; }
        public string YAxis { get; set; }
        public double XMark { get; set; }
        public string MarkColor { get; set; }
    }
}
