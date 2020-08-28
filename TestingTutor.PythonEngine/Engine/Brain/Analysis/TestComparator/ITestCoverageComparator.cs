using System.Collections.Generic;
using TestingTutor.EngineModels;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.Parsers;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.Trace;

namespace TestingTutor.PythonEngine.Engine.Brain.Analysis.TestComparator
{
    public interface ITestCoverageComparator
    {
        void Compare(ref IList<AnnotatedTest> annotatedTests, ref TraceTests instructor, ref TraceTests student,
            ref FeedbackDto feedback);
    }
}
