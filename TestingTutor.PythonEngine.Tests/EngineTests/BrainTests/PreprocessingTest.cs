using System.IO;
using System.IO.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingTutor.EngineModels;
using TestingTutor.PythonEngine.Engine.Brain.Preprocessing;
using TestingTutor.PythonEngine.Engine.Factory;

namespace TestingTutor.PythonEngine.Tests.EngineTests.BrainTests
{
    [TestClass]
    public class PreprocessingTest
    {
        protected IEngineFactory Factory;
        protected string CurrentDirectory = Directory.GetCurrentDirectory();

        [TestInitialize]
        public void Init()
        {
            Factory = new EngineFactory(CurrentDirectory);
        }

        [TestMethod]
        public void GetLearningMode()
        {
            // Arrange
            var mode = "Learning Mode";

            // Act
            var preprocessor = Factory.Preprocessor(mode);

            // Assert
            Assert.AreEqual(preprocessor.GetType(), typeof(LearningPreprocessor));
        }

        [TestMethod]
        public void GetDevelopmentMode()
        {
            // Arrange
            var mode = "Development Mode";

            // Act
            var preprocessor = Factory.Preprocessor(mode);

            // Assert
            Assert.AreEqual(preprocessor.GetType(), typeof(DevelopingPreprocessor));
        }

        [TestMethod]
        public void LearningMode_ShouldPass()
        {
            var submission = new SubmissionDto()
            {
                ApplicationMode = "Learning Mode",
                TestCaseSolution = ConvertToZipByteArray(
                        "def test():\r\n" +
                        "   assert \"a\" in \"abc\"\r\n",
                        "my_test.py"),
                AssignmentSolution = null,
                ReferenceSolution = ConvertToZipByteArray(
                                "def func():\r\n" +
                                "   return 3"
                                , "solution.py"),
                ReferenceTestSolution = ConvertToZipByteArray(
                                "def test():\r\n" +
                                "   assert \"a\" in \"abc\"\r\n",
                                "my_test.py"),
                SolutionFolderName = "Solution"
            };

            
            var preprocessor = Factory.Preprocessor(submission.ApplicationMode);

            var result = preprocessor.Preprocessing(submission,
                CurrentDirectory, out var workingDirectories,
                out var feedback);

            Assert.IsTrue(result);

            Directory.Delete(Path.Combine(CurrentDirectory, "ReferenceTestSuit"), true);
            Directory.Delete(Path.Combine(CurrentDirectory, "Solution"), true);
            Directory.Delete(Path.Combine(CurrentDirectory, "StudentTestSuit"), true);
        }

        [TestMethod]
        public void DevelopingMode_ShouldPass()
        {
            var submission = new SubmissionDto()
            {
                ApplicationMode = "Development Mode",
                TestCaseSolution = ConvertToZipByteArray(
                    "import sys\r\n" +
                    "sys.path.append('../Solution/')\r\n" +
                    "from solution import func\r\n" +
                    "def test():\r\n" +
                    "   assert func() == 3\r\n",
                    "my_test.py"),
                AssignmentSolution = ConvertToZipByteArray(
                        "def func():\r\n" +
                        "   return 3"
                        , "solution.py"),
                ReferenceSolution = ConvertToZipByteArray(
                                "def func():\r\n" +
                                "   return 3"
                                , "solution.py"),
                ReferenceTestSolution = ConvertToZipByteArray(
                                "import sys\r\n" +
                                "sys.path.append('../Solution/')\r\n" +
                                "from solution import func\r\n" +
                                "def test():\r\n" +
                                "   assert func() == 3\r\n",
                                "my_test.py"),
                SolutionFolderName = "Solution"
            };

            var preprocessor = Factory.Preprocessor(submission.ApplicationMode);

            var result = preprocessor.Preprocessing(submission,
                CurrentDirectory, out var workingDirectories,
                out var feedback);

            Assert.IsTrue(result);

            Directory.Delete(Path.Combine(CurrentDirectory, "ReferenceTestSuit"), true);
            Directory.Delete(Path.Combine(CurrentDirectory, "Solution"), true);
            Directory.Delete(Path.Combine(CurrentDirectory, "StudentTestSuit"), true);
        }


        [TestMethod]
        public void DevelopingMode_ShouldFail()
        {
            var submission = new SubmissionDto()
            {
                ApplicationMode = "Development Mode" ,
                TestCaseSolution = ConvertToZipByteArray(
                    "import sys\r\n" +
                    "sys.path.append('../Solution/')\r\n" +
                    "from solution import func\r\n" +
                    "def test_should_fail():\r\n" +
                    "   assert func() == 4\r\n",
                    "my_test.py"),
                AssignmentSolution = ConvertToZipByteArray(
                    "def func():\r\n" +
                    "   return 3"
                    , "solution.py"),
                ReferenceSolution = ConvertToZipByteArray(
                        "def func():\r\n" +
                        "   return 3"
                        , "solution.py"),
                ReferenceTestSolution = ConvertToZipByteArray(
                        "import sys\r\n" +
                        "sys.path.append('../Solution/')\r\n" +
                        "from solution import func\r\n" +
                        "def test():\r\n" +
                        "   assert func() == 3\r\n",
                        "my_test.py"),
                SolutionFolderName = "Solution"
            };

            var preprocessor = Factory.Preprocessor(submission.ApplicationMode);

            var result = preprocessor.Preprocessing(submission,
                CurrentDirectory, out var workingDirectories,
                out var feedback);

            Assert.IsFalse(result);

            Directory.Delete(Path.Combine(CurrentDirectory, "ReferenceTestSuit"), true);
            Directory.Delete(Path.Combine(CurrentDirectory, "Solution"), true);
            Directory.Delete(Path.Combine(CurrentDirectory, "StudentTestSuit"), true);
        }

        [TestMethod]
        public void DevelopingModeNoTestSuit_ShouldFail()
        {
            var submission = new SubmissionDto()
            {
                ApplicationMode = "Development Mode",
                TestCaseSolution = null,
                AssignmentSolution = ConvertToZipByteArray(
                    "def func():\r\n" +
                    "   return 3"
                    , "solution.py"),
                ReferenceSolution = ConvertToZipByteArray(
                            "def func():\r\n" +
                            "   return 3"
                            , "solution.py"),
                ReferenceTestSolution = ConvertToZipByteArray(
                            "import sys\r\n" +
                            "sys.path.append('../Solution/')\r\n" +
                            "from solution import func\r\n" +
                            "def test():\r\n" +
                            "   assert func() == 3\r\n",
                            "my_test.py"),
                SolutionFolderName = "Solution"
            };

            var preprocessor = Factory.Preprocessor(submission.ApplicationMode);

            var result = preprocessor.Preprocessing(submission,
                CurrentDirectory, out var workingDirectories,
                out var feedback);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DevelopingModeNoAssignmentSolution_ShouldFail()
        {
            var submission = new SubmissionDto()
            {
                ApplicationMode = "Development Mode",
                TestCaseSolution = ConvertToZipByteArray(
                    "import sys\r\n" +
                    "sys.path.append('../Solution/')\r\n" +
                    "from solution import func\r\n" +
                    "def test():\r\n" +
                    "   assert func() == 3\r\n",
                    "my_test.py"),
                AssignmentSolution = null,
                ReferenceSolution = ConvertToZipByteArray(
                            "def func():\r\n" +
                            "   return 3"
                            , "solution.py"),
                SolutionFolderName = "Solution",
                ReferenceTestSolution = ConvertToZipByteArray(
                            "import sys\r\n" +
                            "sys.path.append('../Solution/')\r\n" +
                            "from solution import func\r\n" +
                            "def test():\r\n" +
                            "   assert func() == 3\r\n",
                            "my_test.py"),
            };

            var preprocessor = Factory.Preprocessor(submission.ApplicationMode);

            var result = preprocessor.Preprocessing(submission,
                CurrentDirectory, out var workingDirectories,
                out var feedback);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DevelopingModeNoAssignmentReferenceSolution_ShouldFail()
        {
            var submission = new SubmissionDto()
            {
                ApplicationMode = "Development Mode",
                TestCaseSolution = ConvertToZipByteArray(
                    "import sys\r\n" +
                    "sys.path.append('../Solution/')\r\n" +
                    "from solution import func\r\n" +
                    "def test():\r\n" +
                    "   assert func() == 3\r\n",
                    "my_test.py"),
                AssignmentSolution = ConvertToZipByteArray(
                    "def func():\r\n" +
                    "   return 3"
                    , "solution.py"),
                ReferenceSolution = null,
                ReferenceTestSolution = ConvertToZipByteArray(
                            "import sys\r\n" +
                            "sys.path.append('../Solution/')\r\n" +
                            "from solution import func\r\n" +
                            "def test():\r\n" +
                            "   assert func() == 3\r\n",
                            "my_test.py"),
                SolutionFolderName = "Solution"
            };

            var preprocessor = Factory.Preprocessor(submission.ApplicationMode);

            var result = preprocessor.Preprocessing(submission,
                CurrentDirectory, out var workingDirectories,
                out var feedback);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DevelopingModeNoReferenceTestSuit_ShouldFail()
        {
            var submission = new SubmissionDto()
            {
                ApplicationMode = "Development Mode",
                TestCaseSolution = ConvertToZipByteArray(
                    "import sys\r\n" +
                    "sys.path.append('../Solution/')\r\n" +
                    "from solution import func\r\n" +
                    "def test():\r\n" +
                    "   assert func() == 3\r\n",
                    "my_test.py"),
                AssignmentSolution = ConvertToZipByteArray(
                    "def func():\r\n" +
                    "   return 3"
                    , "solution.py"),
                ReferenceSolution = ConvertToZipByteArray(
                        "def func():\r\n" +
                        "   return 3"
                        , "solution.py"),
                ReferenceTestSolution = null,
                SolutionFolderName = "Solution"
            };

            var preprocessor = Factory.Preprocessor(submission.ApplicationMode);

            var result = preprocessor.Preprocessing(submission,
                CurrentDirectory, out var workingDirectories,
                out var feedback);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LearningModeNoTestSuit_ShouldFail()
        {
            var submission = new SubmissionDto()
            {
                ApplicationMode = "Learning Mode",
                TestCaseSolution = null,
                ReferenceSolution = ConvertToZipByteArray(
                            "def func():\r\n" +
                            "   return 3"
                            , "solution.py"),
                ReferenceTestSolution = ConvertToZipByteArray(
                            "def test():\r\n" +
                            "   assert \"a\" in \"abc\"\r\n",
                            "my_test.py"),
                SolutionFolderName = "Solution"
            };

            var preprocessor = Factory.Preprocessor(submission.ApplicationMode);

            var result = preprocessor.Preprocessing(submission,
                CurrentDirectory, out var workingDirectories,
                out var feedback);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LearningModeNoReferenceTestCaseSolution_ShouldFail()
        {
            var submission = new SubmissionDto()
            {
                ApplicationMode = "Learning Mode",
                TestCaseSolution = ConvertToZipByteArray(
                    "def test():\r\n" +
                    "   assert \"a\" in \"abc\"\r\n",
                    "my_test.py"),
                ReferenceSolution = null,
                SolutionFolderName = "Solution",
                ReferenceTestSolution = ConvertToZipByteArray(
                            "def test():\r\n" +
                            "   assert \"a\" in \"abc\"\r\n",
                            "my_test.py"),
            };

            var preprocessor = Factory.Preprocessor(submission.ApplicationMode);

            var result = preprocessor.Preprocessing(submission,
                CurrentDirectory, out var workingDirectories,
                out var feedback);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LearningModeNoReferenceTestSuit_ShouldFail()
        {
            var submission = new SubmissionDto()
            {
                ApplicationMode = "Learning Mode",
                TestCaseSolution = ConvertToZipByteArray(
                    "def test():\r\n" +
                    "   assert \"a\" in \"abc\"\r\n",
                    "my_test.py"),
                ReferenceSolution = ConvertToZipByteArray(
                            "def func():\r\n" +
                            "   return 3"
                            , "solution.py"),
                ReferenceTestSolution = null,
                SolutionFolderName = "Solution"
            };

            var preprocessor = Factory.Preprocessor(submission.ApplicationMode);

            var result = preprocessor.Preprocessing(submission,
                CurrentDirectory, out var workingDirectories,
                out var feedback);

            Assert.IsFalse(result);
        }


        private byte[] ConvertToZipByteArray(string code, string name)
        {
            var tempDir = Path.Combine(CurrentDirectory, "TempDir");
            Directory.CreateDirectory(tempDir);
            var tempFile = Path.Combine(tempDir, name);
            File.WriteAllLines(tempFile, new[] { code });
            var zipPath = Path.Combine(CurrentDirectory, "temp.zip");
            ZipFile.CreateFromDirectory(tempDir, zipPath);
            byte[] contents = File.ReadAllBytes(zipPath);
            Directory.Delete(Path.Combine(CurrentDirectory, "TempDir"), true);
            File.Delete(zipPath);
            return contents;
        }

        [TestCleanup]
        public void Cleanup()
        {

        }
    }
}
