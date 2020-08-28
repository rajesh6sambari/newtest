using System.Collections.Generic;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.Parsers;

namespace TestingTutor.PythonEngine.Engine.Brain.Analysis.Trace
{
    public interface ITracer
    {
        TestCoverage GetTestCoverage(string testsuit, string coverDir, IList<IndividualTest> tests);
    }
}
