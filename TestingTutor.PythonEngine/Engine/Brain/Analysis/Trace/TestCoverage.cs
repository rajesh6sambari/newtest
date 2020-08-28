using System.Collections.Generic;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.Parsers;

namespace TestingTutor.PythonEngine.Engine.Brain.Analysis.Trace
{
    public class TestCoverage
    {
        public IndividualTest Test { get; set; }
        public bool Pass { get; set; }
        public IList<ModuleCoverage> ModuleCoverages { get; set; } = new List<ModuleCoverage>();
    }
}
