using System.Collections.Generic;

namespace TestingTutor.UI.DataVisuals
{
    public class ThresholdMultiLineChartLine
    {
        public string Color { get; set; }
        public string Label { get; set; }
        public IList<ChartPoint> Points { get; set; } = new List<ChartPoint>();
    }
}