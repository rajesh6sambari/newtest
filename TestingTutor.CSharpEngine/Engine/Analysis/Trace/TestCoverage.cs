using System.Collections.Generic;
using TestingTutor.CSharpEngine.Engine.Analysis.Parser;
using OpenCover.Framework.Model;

namespace TestingTutor.CSharpEngine.Engine.Analysis.Trace
{
    public class TestCoverage
    {
        public IndividualTest Test { get; set; }
        public bool Pass { get; set; }
        public IList<ModuleCoverage> ModuleCoverages { get; set; } = new List<ModuleCoverage>();
        public CoverageSession CoverageSession { get; set; } = new CoverageSession();
    }
}
