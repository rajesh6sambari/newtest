using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.CSharpEngine.Engine.Analysis.Parser;
using TestingTutor.CSharpEngine.Engine.Analysis.Trace;
using TestingTutor.EngineModels;
using TestingTutor.CSharpEngine.Models;

namespace TestingTutor.CSharpEngine.Engine.Analysis.TestComparator
{
    public interface ITestCoverageComparator
    {
        void Compare(ref IList<AnnotatedTest> annotatedTests, ref TraceTests instructor, ref TraceTests student,
            ref FeedbackDto feedback, EngineWorkingDirectories workingDirectories);
    }
}
