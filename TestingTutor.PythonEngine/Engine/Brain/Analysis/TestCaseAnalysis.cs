using System.Collections.Generic;
using System.IO;
using TestingTutor.EngineModels;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.Parsers;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.Trace;
using TestingTutor.PythonEngine.Engine.Factory;
using TestingTutor.PythonEngine.Engine.Models;

namespace TestingTutor.PythonEngine.Engine.Brain.Analysis
{
    public class TestCaseAnalysis : ITestCaseAnalysis
    {
        protected IEngineFactory EngineFactory;

        public TestCaseAnalysis(IEngineFactory engineFactory)
        {
            EngineFactory = engineFactory;
        }

        public void Run(SubmissionDto submissionDto, ref EngineWorkingDirectories workingDirectories, ref FeedbackDto feedback)
        {
            var instructorTestSuit = CollectTestCases(workingDirectories.ReferenceTestSuit);
            var studentTestSuit = CollectTestCases(workingDirectories.StudentTestSuit);

            var annotatedTestSuit = CollectAnnotatedTests(workingDirectories.ReferenceTestSuit, ref instructorTestSuit);

            var modulesNams = new ModuleHandlers.ModuleHandler().GetModuleNames(workingDirectories.Solution);

            var coverDirectory = Path.Combine(workingDirectories.ParentDirectory, "CoverDirectory");
            Directory.CreateDirectory(coverDirectory);

            var tracer = new Tracer(EngineFactory);
            var instructorTestCoverage = tracer.GetTestCoverage(workingDirectories.ReferenceTestSuit,
                coverDirectory, instructorTestSuit, modulesNams);
            var studentTestCoverage = tracer.GetTestCoverage(workingDirectories.StudentTestSuit,
                coverDirectory, studentTestSuit, modulesNams);

            var comparator = EngineFactory.Comparator();
            comparator.Compare(ref annotatedTestSuit, ref instructorTestCoverage, ref studentTestCoverage,
                ref feedback);
        }
        
        public IList<IndividualTest> CollectTestCases(string directory)
        {
            var pytest = EngineFactory.Pytest();

            pytest.Run("--collect-only", "CollectTestTemp.txt", directory);

            var content = File.ReadAllLines(Path.Combine(directory, "CollectTestTemp.txt"));

            File.Delete(Path.Combine(directory, "CollectTestTemp.txt"));

            return new TestCaseParser().GatherTests(content);
        }

        public IList<AnnotatedTest> CollectAnnotatedTests(string directory, ref IList<IndividualTest> tests)
        {
            var pytest = EngineFactory.Pytest();
            var parser = new AnnotatedTestParser();
            var temp = Path.Combine(directory, "annotated_test.xml");

            var annotatedTests = new List<AnnotatedTest>();

            foreach (var test in tests)
            {
                pytest.Run($"{test.TestName} --junitxml={temp}", "$null", directory);
                var annotation = parser.GatherAnnotatedTest(temp);
                annotation.IndividualTest = test;
                annotatedTests.Add(annotation);
            }

            File.Delete(temp);
            return annotatedTests;
        }
    }
}
