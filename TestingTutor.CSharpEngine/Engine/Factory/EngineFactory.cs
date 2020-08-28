using System;
using TestingTutor.CSharpEngine.FileHandling;
using TestingTutor.CSharpEngine.OpenCover;
using TestingTutor.CSharpEngine.Engine.Preprocessing;
using TestingTutor.CSharpEngine.Engine.Analysis.TestComparator;
using TestingTutor.CSharpEngine.Engine.Utilities.Workspace;
using TestingTutor.CSharpEngine.Engine.Analysis;
using TestingTutor.CSharpEngine.Engine.CoverageStat;
using TestingTutor.CSharpEngine.Engine.Feedback;
using System.IO;

namespace TestingTutor.CSharpEngine.Engine.Factory
{
    public class EngineFactory: IEngineFactory
    {
        //TODO: clean up paths and make them direct
        protected string Root;
        protected string OpenCoverPath = @"C:\Users\" + "@"+Environment.UserName +@"\.nuget\packages\opencover\4.7.922\tools\OpenCover.Console.exe";

        public EngineFactory(string root)
        {
            Root = root;
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

        public ITrace Trace()
        {
            return new OpenTrace(OpenCoverPath);
        }

        public ITestCoverageComparator Comparator()
        {
            return new TestCoverageComparator();
        }

        public IWorkspace Workspace(string identifier)
        {
            return new TempWorkspace(Path.Combine(Root, identifier));
        }

        public ITestCaseAnalysis TestCaseAnalysis()
        {
            return new TestCaseAnalysis(this);
        }

        public ICoverageStats CoverageStats()
        {
            return new CoverageStats(this);
        }

        public IFeedbackSender FeedbackSender()
        {
            return new FeedbackSender();
        }
    }
}
