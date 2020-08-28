using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingTutor.UI.DataVisuals
{
    public class MultiLineChart
    {
        public string Id { get; set; }
        public IList<IList<int>> Lines { get; set; } = new List<IList<int>>();
        public IList<string> Colors { get; set; } = new List<string>();
        public IList<string> Labels { get; set; } = new List<string>();
        public IList<int> Domain { get; set; } = new List<int>();
        public int Minimum { get; set; }
        public int Maximum { get; set; }
    }
}
