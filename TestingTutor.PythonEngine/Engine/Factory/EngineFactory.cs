using System;
using System.IO;
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
    public class EngineFactory : IEngineFactory
    {
        protected string Root;
        protected const string PytestPath = @"C:\Pytest\pytest.py";

        public EngineFactory(string root)
        {
            Root = root;
        }

        public IWorkspace Workspace(string identifier)
        {
            return new TempWorkspace(Path.Combine(Root, identifier));
        }

        public IFileHandler FileHandler()
        {
            return new FileHandler();
        }

        public IPreprocessor Preprocessor(string mode)
        {
            switch (mode)
            {
                case "Learning Mode":
                    return new LearningPreprocessor(this);
                case "Development Mode":
                    return new DevelopingPreprocessor(this);
            }

            throw new ArgumentException("Mode doesn't exist");
        }

        public IPytest Pytest()
        {
            return new Pytest();
        }

        public IFeedbackSender FeedbackSender()
        {
            return new FeedbackSender();
        }

        public ITestCaseAnalysis TestCaseAnalysis()
        {
            return new TestCaseAnalysis(this);
        }

        public ITrace Trace()
        {
            return new PyTracer(PytestPath);
        }

        public ITestCoverageComparator Comparator()
        {
            return new TestCoverageComparator();
        }

        public ICoverageStats CoverageStats()
        {
            return new CoverageStats(this);
        }

        public IPytestCov PytestCov()
        {
            return new PytestCov();
        }
    }
}
