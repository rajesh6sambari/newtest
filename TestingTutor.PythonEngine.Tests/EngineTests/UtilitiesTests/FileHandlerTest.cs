using System.IO;
using System.IO.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingTutor.PythonEngine.Engine.Utilities.FileHandlers;

namespace TestingTutor.PythonEngine.Tests.EngineTests.UtilitiesTests
{
    [TestClass]
    public class FileHandlerTest
    {
        protected string CurrentDirectory;

        [TestInitialize]
        public void Init()
        {
            CurrentDirectory = Directory.GetCurrentDirectory();
        }

        [TestMethod]
        public void CheckIfFileExist()
        {
            // Arrange
            var tempDir = Path.Combine(CurrentDirectory, "TempDir");
            Directory.CreateDirectory(tempDir);
            var tempFile = Path.Combine(tempDir, "TempFile.txt");
            File.WriteAllLines(tempFile, new [] {"Some random text"});
            var zipPath = Path.Combine(CurrentDirectory, "temp.zip");
            ZipFile.CreateFromDirectory(tempDir, zipPath);
            byte[] contents = File.ReadAllBytes(zipPath);
            var handler = new FileHandler();
            var newLocation = Path.Combine(CurrentDirectory, "ResultDir");

            // Act
            handler.UnzipByteArray(contents, CurrentDirectory, newLocation);

            // Assert
            Assert.IsTrue(File.Exists(Path.Combine(newLocation, "TempFile.txt")));

            // Cleanup
            Directory.Delete(Path.Combine(CurrentDirectory, "TempDir"), true);
            Directory.Delete(Path.Combine(CurrentDirectory, "ResultDir"), true);
        }
    }
}
