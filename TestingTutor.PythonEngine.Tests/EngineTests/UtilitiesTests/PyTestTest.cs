using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingTutor.PythonEngine.Engine.Utilities.Python;

namespace TestingTutor.PythonEngine.Tests.EngineTests.UtilitiesTests
{
    [TestClass]
    public class PyTestTest
    {
        protected IPytest Pytest;
        protected string CurrentDirectory;

        [TestInitialize]
        public void Init()
        {
            Pytest = new Pytest();
            CurrentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "PyTestTemp");
            Directory.CreateDirectory(CurrentDirectory);
        }

        [TestMethod]
        public void PyTest_ShouldPass()
        {
            // Arrange
            var file = Path.Combine(CurrentDirectory, "temp.py");
            File.WriteAllText(file,
                "def func(x):\r\n" +
                "    return x + 1\r\n" +
                "\r\n" +
                "def test_should_pass():\r\n" +
                "    expected = 4\r\n" +
                "    actual = func(3)\r\n" +
                "    assert actual == expected");

            // Act
            var exitCode = Pytest.Run(file, "$null", CurrentDirectory);

            // Assert
            Assert.AreEqual(0, exitCode);
        }

        [TestMethod]
        public void PyTest_ShouldFail()
        {
            // Arrange
            var file = Path.Combine(CurrentDirectory, "temp.py");
            File.WriteAllText(file,
                "def func(x):\r\n" +
                "    return x + 1\r\n" +
                "\r\n" +
                "def test_should_fail():\r\n" +
                "    expected = 5\r\n" +
                "    actual = func(3)\r\n" +
                "    assert actual == expected");

            // Act
            var exitCode = Pytest.Run(file, "$null", CurrentDirectory);

            // Assert
            Assert.AreNotEqual(0, exitCode);
            
        }
        
        [TestMethod]
        public void PyTestJunit_ShouldPass()
        {
            // Arrange
            var file = Path.Combine(CurrentDirectory, "temp.py");
            File.WriteAllText(file,
                "def func(x):\r\n" +
                "    return x + 1\r\n" +
                "\r\n" +
                "def test_should_fail():\r\n" +
                "    expected = 5\r\n" +
                "    actual = func(3)\r\n" +
                "    assert actual == expected");
            var xml = Path.Combine(CurrentDirectory, "thing.xml");

            // Act
            Pytest.Run($"temp.py::test_should_fail --junitxml={xml}", "$null", CurrentDirectory);

            // Assert
            Assert.IsTrue(File.Exists(xml));
            
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(CurrentDirectory, true);
        }
    }
}
