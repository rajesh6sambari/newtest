using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingTutor.UI.DataVisuals
{
    public class LabelLineChart
    {
        public string Id { get; set; }
        public IList<double> XValues { get; set; } = new List<double>();
        public IList<double> YValues { get; set; } = new List<double>();
        public IList<string> Colors { get; set; } = new List<string>();
        public string XAxis { get; set; }
        public string YAxis { get; set; }
    }
}
