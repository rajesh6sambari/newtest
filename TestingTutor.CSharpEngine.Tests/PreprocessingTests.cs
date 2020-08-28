using System;
using System.Collections.Generic;
using System.Text;
using TestingTutor.EngineModels;
using TestingTutor.CSharpEngine.Engine;
using System.IO;
using System.IO.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingTutor.CSharpEngine.Engine.Preprocessing;
using TestingTutor.CSharpEngine.Engine.Factory;
using TestingTutor.CSharpEngine.FileHandling;

namespace TestingTutor.CSharpEngine.Tests
{
    [TestClass]
    public class PreprocessingTests
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
        public void LearningModePassCase()
        {
            var InstructorBytes = File.ReadAllBytes(@"C:\Users\Admin\Desktop\ReferenceSolution.zip");
            var StudentBytes = File.ReadAllBytes(@"C:\Users\Admin\Desktop\StudentSolution.zip");

            var submission = new SubmissionDto()
            {
                ApplicationMode = "Learning Mode",
                TestCaseSolution = ConvertToZipByteArray(@"C:\Users\Nate\Desktop\VSTT-Demo\TestBank\bin\Debug\TestBank.dll"
                   ,"TestBank.dll" ),
                AssignmentSolution = ConvertToZipByteArray(@"C:\Users\Nate\Desktop\VSTT-Demo\Bank\bin\Debug\Bank.dll","Bank.dll"
                        ),
                ReferenceSolution = ConvertToZipByteArray(@"C:\Users\Nate\Desktop\VSTT-Demo\TestBank\bin\Debug\TestBank.dll","TestBank.dll"
                                ),
                ReferenceTestSolution = ConvertToZipByteArray(@"C:\Users\Nate\Desktop\VSTT-Demo\Bank\bin\Debug\Bank.dll", "Bank.dll"
                                ),
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
        public void LearningModeWithSolutions()
        {
            var InstructorBytes = File.ReadAllBytes(@"C:\Users\Admin\Desktop\ReferenceSolution.zip");
            var StudentBytes = File.ReadAllBytes(@"C:\Users\Admin\Desktop\StudentSolution.zip");

            var fileHandler = new FileHandler();
            var newLocation = Path.Combine(CurrentDirectory, "ResultDir");

            var submission = new SubmissionDto()
            {
                ApplicationMode = "Learning Mode",
                TestCaseSolution = StudentBytes,

                AssignmentSolution = StudentBytes,

                ReferenceSolution = InstructorBytes,
                ReferenceTestSolution = InstructorBytes, 

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
    }
}
