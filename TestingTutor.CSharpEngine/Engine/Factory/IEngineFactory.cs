using TestingTutor.CSharpEngine.FileHandling;
using TestingTutor.CSharpEngine.OpenCover;
using TestingTutor.CSharpEngine.Engine.Preprocessing;
using TestingTutor.CSharpEngine.Engine.Analysis.TestComparator;
using TestingTutor.CSharpEngine.Engine.Utilities.Workspace;
using TestingTutor.CSharpEngine.Engine.Analysis;
using TestingTutor.CSharpEngine.Engine.CoverageStat;
using TestingTutor.CSharpEngine.Engine.Feedback;

namespace TestingTutor.CSharpEngine.Engine.Factory
{
    public interface IEngineFactory
    {
        IWorkspace Workspace(string identifier);
        IFileHandler FileHandler();
        IPreprocessor Preprocessor(string mode);
        ITrace Trace();
        ITestCoverageComparator Comparator();
        ITestCaseAnalysis TestCaseAnalysis();
        ICoverageStats CoverageStats();
        IFeedbackSender FeedbackSender();
    }
}
