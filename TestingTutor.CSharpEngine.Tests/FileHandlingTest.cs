using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.IO.Compression;
using TestingTutor.CSharpEngine.FileHandling;


namespace TestingTutor.CSharpEngine.Tests
{
    [TestClass]
    public class FileHandlingTest
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
            File.WriteAllLines(tempFile, new[] { "Some random text" });
            var zipPath = Path.Combine(CurrentDirectory, "temp.zip");
            ZipFile.CreateFromDirectory(tempDir, zipPath);
            byte[] contents = File.ReadAllBytes(zipPath);
            var fileHandler = new FileHandler();
            var newLocation = Path.Combine(CurrentDirectory, "ResultDir");

            // Act
            fileHandler.UnzipArray(contents, CurrentDirectory, newLocation);

            // Assert
            Assert.IsTrue(File.Exists(Path.Combine(newLocation, "TempFile.txt")));

            // Cleanup
            Directory.Delete(Path.Combine(CurrentDirectory, "TempDir"), true);
            Directory.Delete(Path.Combine(CurrentDirectory, "ResultDir"), true);
        }
    }
}

