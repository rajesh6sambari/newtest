using TestingTutor.PythonEngine.Engine.Brain.Analysis;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.TestComparator;
using TestingTutor.PythonEngine.Engine.Brain.CoverageStat;
using TestingTutor.PythonEngine.Engine.Brain.Feedback;
using TestingTutor.PythonEngine.Engine.Brain.Preprocessing;
using TestingTutor.PythonEngine.Engine.Utilities;
using TestingTutor.PythonEngine.Engine.Utilities.FileHandlers;
using TestingTutor.PythonEngine.Engine.Utilities.Python;
using TestingTutor.PythonEngine.Engine.Utilities.Workspaces;

namespace TestingTutor.PythonEngine.Engine.Factory
{
    public interface IEngineFactory
    {
        IWorkspace Workspace(string identifier);
        IFileHandler FileHandler();
        IPreprocessor Preprocessor(string mode);
        IPytest Pytest();
        IFeedbackSender FeedbackSender();
        ITestCaseAnalysis TestCaseAnalysis();
        ITrace Trace();
        ITestCoverageComparator Comparator();
        ICoverageStats CoverageStats();
        IPytestCov PytestCov();
    }
}
