using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingTutor.UI.DataVisuals
{
    public class MultiRadarChart
    {
        public string Id { get; set; }
        public IList<MultiRadarChartAxis> MultiRadarChartAxises { get; set; } = new List<MultiRadarChartAxis>();
        public IList<string> Colors { get; set; }
        public double Maximum { get; set; }
        public double Minimum { get; set; }
    }

    public class MultiRadarChartAxis
    {
        public string Axis { get; set; }
        public IList<double> Values { get; set; } = new List<double>();
    }
}
