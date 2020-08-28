using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingTutor.PythonEngine.Engine.Utilities.Python;

namespace TestingTutor.PythonEngine.Tests.EngineTests.BrainTests.AnalysisTests
{
    [TestClass]
    public class PyTracerTest
    {
        protected string CurrentDirectory;
        protected ITrace Tracer;

        [TestInitialize]
        public void Init()
        {
            var temp = Directory.GetCurrentDirectory();
            CurrentDirectory = Path.Combine(temp, "PyTracer");
            Directory.CreateDirectory(CurrentDirectory);

            Tracer = new PyTracer(@"C:\PyTest\pytest.py");
        }

        [TestMethod]
        public void GotCoverage_ShouldPass()
        {
            // Arrange
            var file = Path.Combine(CurrentDirectory, "my_test.py");
            File.WriteAllText(file,
                "def test_one():\r\n" +
                "   assert \"a\" in \"abc\"");

            // Act
            Tracer.Run("my_test.py::test_one", CurrentDirectory, "$null", CurrentDirectory);

            // Assert
            var coverage = Path.Combine(CurrentDirectory, "my_test.cover");
            Assert.IsTrue(File.Exists(coverage));
        }

        [TestMethod]
        public void GotCoverage_ShouldHaveFailTest()
        {
            // Arrange
            var file = Path.Combine(CurrentDirectory, "my_test.py");
            File.WriteAllText(file,
                "def test_one():\r\n" +
                "   assert \"e\" in \"abc\"");
            var outputFile = Path.Combine(CurrentDirectory, "Temp.txt");

            // Act
            Tracer.Run("my_test.py::test_one", CurrentDirectory, outputFile, CurrentDirectory);

            // Assert
            var contents = File.ReadAllText(outputFile);
            Assert.IsTrue(Regex.IsMatch(contents, $"FAILURES"));
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(CurrentDirectory, true);
        }
    }
}
