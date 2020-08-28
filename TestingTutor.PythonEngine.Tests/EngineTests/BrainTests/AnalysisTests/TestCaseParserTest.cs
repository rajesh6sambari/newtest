using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.Parsers;
using TestingTutor.PythonEngine.Engine.Utilities.Python;

namespace TestingTutor.PythonEngine.Tests.EngineTests.BrainTests.AnalysisTests
{
    [TestClass]
    public class TestCaseParserTest
    {
        protected string WorkingDirectory;
        protected TestCaseParser Parser;
        protected IPytest Pytest;

        [TestInitialize]
        public void Init()
        {
            var current = Directory.GetCurrentDirectory();
            WorkingDirectory = Path.Combine(current, "TestCaseParser");
            Directory.CreateDirectory(WorkingDirectory);

            Pytest = new Pytest();
            Parser = new TestCaseParser();
        }

        [TestMethod]
        public void SingleFunctionTest_ShouldPass()
        {
            // Arrange
            var file = Path.Combine(WorkingDirectory, "my_test.py");
            File.WriteAllText(file,
                "def test_simple():\r\n" +
                "   assert \"a\" in \"abc\"");
            Pytest.Run("--collect-only", "CollectTestTemp.txt", WorkingDirectory);
            var content = File.ReadAllLines(Path.Combine(WorkingDirectory, "CollectTestTemp.txt"));
            File.Delete(Path.Combine(WorkingDirectory, "CollectTestTemp.txt"));
            var expected = new List<IndividualTest>()
            {
                new IndividualTest("my_test.py::test_simple"),
            };

            // Act
            var tests = Parser.GatherTests(content);

            // Assert
            Assert.IsTrue(EqualTest(expected,tests));
        }

        [TestMethod]
        public void MultipleFunctionTest_ShouldPass()
        {
            // Arrange
            var file = Path.Combine(WorkingDirectory, "my_test.py");
            File.WriteAllText(file,
                "def test_simple():\r\n" +
                "   assert \"a\" in \"abc\"\r\n" +
                "def test_two():\r\n" +
                "   assert 2 == 2");
            Pytest.Run("--collect-only", "CollectTestTemp.txt", WorkingDirectory);
            var content = File.ReadAllLines(Path.Combine(WorkingDirectory, "CollectTestTemp.txt"));
            File.Delete(Path.Combine(WorkingDirectory, "CollectTestTemp.txt"));
            var expected = new List<IndividualTest>()
            {
                new IndividualTest("my_test.py::test_simple"),
                new IndividualTest("my_test.py::test_two"),
            };

            // Act
            var tests = Parser.GatherTests(content);

            // Assert
            Assert.IsTrue(EqualTest(expected, tests));
        }

        [TestMethod]
        public void MultipleFilesTest_ShouldPass()
        {
            // Arrange
            var file1 = Path.Combine(WorkingDirectory, "first_test.py");
            File.WriteAllText(file1,
                "def test_simple():\r\n" +
                "   assert \"a\" in \"abc\"\r\n" +
                "def test_two():\r\n" +
                "   assert 2 == 2");
            var file2 = Path.Combine(WorkingDirectory, "second_test.py");
            File.WriteAllText(file2,
                "def test_simple():\r\n" +
                "   assert \"a\" in \"abc\"\r\n");
            Pytest.Run("--collect-only", "CollectTestTemp.txt", WorkingDirectory);
            var content = File.ReadAllLines(Path.Combine(WorkingDirectory, "CollectTestTemp.txt"));
            File.Delete(Path.Combine(WorkingDirectory, "CollectTestTemp.txt"));
            var expected = new List<IndividualTest>()
            {
                new IndividualTest("first_test.py::test_simple"),
                new IndividualTest("first_test.py::test_two"),
                new IndividualTest("second_test.py::test_simple"),
            };

            // Act
            var tests = Parser.GatherTests(content);

            // Assert
            Assert.IsTrue(EqualTest(expected, tests));
        }

        [TestMethod]
        public void TestClass_ShouldPass()
        {
            // Arrange
            var file1 = Path.Combine(WorkingDirectory, "my_test.py");
            File.WriteAllText(file1,
                "class TestCalc():\r\n" +
                "   def test_add_pos(self):\r\n" +
                "       assert 1 + 2 == 3\r\n" +
                "   def test_subt_pos(self):\r\n" +
                "       assert 3 - 2 == 1");
            Pytest.Run("--collect-only", "CollectTestTemp.txt", WorkingDirectory);
            var content = File.ReadAllLines(Path.Combine(WorkingDirectory, "CollectTestTemp.txt"));
            File.Delete(Path.Combine(WorkingDirectory, "CollectTestTemp.txt"));
            var expected = new List<IndividualTest>()
            {
                new IndividualTest("my_test.py::TestCalc::test_add_pos"),
                new IndividualTest("my_test.py::TestCalc::test_subt_pos"),
            };

            // Act
            var tests = Parser.GatherTests(content);

            // Assert
            Assert.IsTrue(EqualTest(expected, tests));
        }

        [TestMethod]
        public void TestClassWithFunction_ShouldPass()
        {
            // Arrange
            var file1 = Path.Combine(WorkingDirectory, "my_test.py");
            File.WriteAllText(file1,
                "class TestCalc():\r\n" +
                "   def test_add_pos():\r\n" +
                "       assert 1 + 2 == 3\r\n" +
                "   def test_subt_pos():\r\n" +
                "       assert 3 - 2 == 1\r\n" +
                "def test_one():\r\n" +
                "   assert \"a\" in \"abc\"");
            Pytest.Run("--collect-only", "CollectTestTemp.txt", WorkingDirectory);
            var content = File.ReadAllLines(Path.Combine(WorkingDirectory, "CollectTestTemp.txt"));
            File.Delete(Path.Combine(WorkingDirectory, "CollectTestTemp.txt"));
            var expected = new List<IndividualTest>()
            {
                new IndividualTest("my_test.py::TestCalc::test_add_pos"),
                new IndividualTest("my_test.py::TestCalc::test_subt_pos"),
                new IndividualTest("my_test.py::test_one"),
            };

            // Act
            var tests = Parser.GatherTests(content);

            // Assert
            Assert.IsTrue(EqualTest(expected, tests));
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(WorkingDirectory))
            {
                Directory.Delete(WorkingDirectory, true);
            }
        }

        private bool EqualTest(IList<IndividualTest> left, IList<IndividualTest> right)
        {
            if (left.Count != right.Count) return false;

            var first = left.Except(right).ToList();
            var second = right.Except(left).ToList();

            return !first.Any() && !second.Any();
        }
    }
}
