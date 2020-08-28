using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingTutor.CSharpEngine.Engine.Factory;
using TestingTutor.CSharpEngine.FileHandling;
using System.IO;
using TestingTutor.EngineModels;



namespace TestingTutor.CSharpEngine.Tests
{
    [TestClass]
    public class CSharpEngineTests
    {
        protected IEngineFactory Factory;
        protected IEngine Engine;
        protected string CurrentDirectory = Directory.GetCurrentDirectory();

        [TestInitialize]
        public void Init()
        {
            Factory = new EngineFactory(CurrentDirectory);
        }

   
        [TestMethod]
        public void LearningModeWithSolutions()
        {
            Engine = new Engine.CSharpEngine(Factory);

            var refSolutionPath = Path.Combine(CurrentDirectory + "\\TestSolutions\\ReferenceSolution.zip");
            var refTestSolutionPath = Path.Combine(CurrentDirectory + "\\TestSolutions\\ReferenceTestSolution.zip");
            var studentTestPath = Path.Combine(CurrentDirectory + "\\TestSolutions\\StudentTestSolution.zip");

            var InstructorSolutionBytes = File.ReadAllBytes(refSolutionPath);
            var StudentTestBytes = File.ReadAllBytes(studentTestPath);
            var InstructorTestSolutionBytes = File.ReadAllBytes(refTestSolutionPath);

            var fileHandler = new FileHandler();
            var newLocation = Path.Combine(CurrentDirectory, "ResultDir");

            //Act

            var submission = new SubmissionDto()
            {
                ApplicationMode = "Learning Mode",
                TestCaseSolution = StudentTestBytes,

                AssignmentSolution = null,

                ReferenceSolution = InstructorSolutionBytes,
                ReferenceTestSolution = InstructorTestSolutionBytes,

                SolutionFolderName = "Solution"
            };

            var task = Engine.Run(submission);
            Assert.IsTrue(task.IsCompleted);
        }

        [TestMethod]
        public void LearningModeWithSolutionsLessTests()
        {
            Engine = new Engine.CSharpEngine(Factory);

            var refSolutionPath = Path.Combine(CurrentDirectory + "\\TestSolutions\\ReferenceSolution.zip");
            var refTestSolutionPath = Path.Combine(CurrentDirectory + "\\TestSolutions\\ReferenceTestSolutionLessTests.zip");
            var studentTestPath = Path.Combine(CurrentDirectory + "\\TestSolutions\\StudentTestSolutionLessTests.zip");

            var InstructorSolutionBytes = File.ReadAllBytes(refSolutionPath);
            var StudentTestBytes = File.ReadAllBytes(studentTestPath);
            var InstructorTestSolutionBytes = File.ReadAllBytes(refTestSolutionPath);

            var fileHandler = new FileHandler();
            var newLocation = Path.Combine(CurrentDirectory, "ResultDir");

            //Act

            var submission = new SubmissionDto()
            {
                ApplicationMode = "Learning Mode",
                TestCaseSolution = StudentTestBytes,

                AssignmentSolution = null,

                ReferenceSolution = InstructorSolutionBytes,
                ReferenceTestSolution = InstructorTestSolutionBytes,

                SolutionFolderName = "Solution"
            };

            var task = Engine.Run(submission);
            Assert.IsTrue(task.IsCompleted);
        }
    }
}
