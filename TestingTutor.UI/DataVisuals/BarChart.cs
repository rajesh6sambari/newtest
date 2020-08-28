using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingTutor.UI.DataVisuals
{
    public class BarChart
    {
        public string Id { get; set; }
        public IList<double> Values { get; set; } = new List<double>();
        public IList<string> Colors { get; set; } = new List<string>();
        public IList<string> Labels { get; set; } = new List<string>();
        public double Minimum { get; set; }
    }
}
