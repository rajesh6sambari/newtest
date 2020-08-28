using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.CSharpEngine.Engine.Analysis.Parser;
using TestingTutor.CSharpEngine.Engine.Analysis.ModuleHandlers;

namespace TestingTutor.CSharpEngine.Engine.Analysis.Trace
{
    public interface ITracer
    {
        TraceTests GetTestCoverage(string testsuit,
            string coverDir,
            IList<IndividualTest> tests
           );
    }
}
