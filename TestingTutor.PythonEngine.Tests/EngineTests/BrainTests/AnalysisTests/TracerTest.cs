using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.ModuleHandlers;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.Parsers;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.Trace;
using TestingTutor.PythonEngine.Engine.Factory;

namespace TestingTutor.PythonEngine.Tests.EngineTests.BrainTests.AnalysisTests
{
    [TestClass]
    public class TracerTest
    {
        protected Tracer Tracer;
        protected IEngineFactory Factory;
        protected string CurrentDirectory;

        [TestInitialize]
        public void Init()
        {
            CurrentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TracerTestTemp");
            Directory.CreateDirectory(CurrentDirectory);
            Factory = new EngineFactory(CurrentDirectory);
            Tracer = new Tracer(Factory);
        }

        [TestMethod]
        public void GetTestCoverageFromSingleFile_ShouldPass()
        {
            // Arrange
            var file = Path.Combine(CurrentDirectory, "my_test.py");
            var coverDir = Path.Combine(CurrentDirectory, "CoverDir");
            Directory.CreateDirectory(coverDir);
            File.WriteAllText(file,
                "def test_one():\r\n" +
                "   assert 2 == 2\r\n" +
                "def test_two():\r\n" + 
                "   assert \"d\" in \"abc\"");

            // Act
            var actual = Tracer.GetTestCoverage(CurrentDirectory,
                coverDir,
                new List<IndividualTest>()
                {
                    new IndividualTest("my_test.py::test_one"),
                    new IndividualTest("my_test.py::test_two"),
                },
                new List<ModuleName>()
                {
                    new ModuleName()
                    {
                        Name = "my_test.py"
                    }
                });

            // Assert
            var expected = new TraceTests
            {
                TestCoverages = new List<TestCoverage>()
                {
                    new TestCoverage()
                    {
                        Test = new IndividualTest("my_test.py::test_one"),
                        Pass = true,
                        ModuleCoverages = new List<ModuleCoverage>()
                        {
                            new ModuleCoverage()
                            {
                                ModuleName = new ModuleName()
                                {
                                    Name = "my_test.py"
                                },
                                Contents = new[] {""}
                            }
                        }
                    },
                    new TestCoverage()
                    {
                        Test = new IndividualTest("my_test.py::test_two"),
                        Pass = false,
                        ModuleCoverages = new List<ModuleCoverage>()
                        {
                            new ModuleCoverage()
                            {
                                ModuleName = new ModuleName()
                                {
                                    Name = "my_test.py"
                                },
                                Contents = new[] {""}
                            }
                        }
                    }
                }
            };

            Assert.IsTrue(CompareTraceTest(expected, actual));
        }

        [TestMethod]
        public void GetTestCoverageFromMultipleFile_ShouldPass()
        {
            // Arrange
            var file1 = Path.Combine(CurrentDirectory, "first_test.py");
            var coverDir = Path.Combine(CurrentDirectory, "CoverDir");
            Directory.CreateDirectory(coverDir);
            File.WriteAllText(file1,
                "def test_one():\r\n" +
                "   assert 2 == 2\r\n" +
                "def test_two():\r\n" +
                "   assert \"d\" in \"abc\"");
            var file2 = Path.Combine(CurrentDirectory, "second_test.py");
            File.WriteAllText(file2,
                "def test_three():\r\n" +
                "   assert 1 != 2\r\n");

            // Act
            var actual = Tracer.GetTestCoverage(CurrentDirectory,
                coverDir,
                new List<IndividualTest>()
                {
                    new IndividualTest("first_test.py::test_one"),
                    new IndividualTest("first_test.py::test_two"),
                    new IndividualTest("second_test.py::test_three"),
                },
                new List<ModuleName>()
                {
                    new ModuleName()
                    {
                        Name = "first_test.py"
                    },
                    new ModuleName()
                    {
                        Name = "second_test.py"
                    },
                });

            // Assert
            var expected = new TraceTests
            {
                TestCoverages = new List<TestCoverage>()
                {
                    new TestCoverage()
                    {
                        Test = new IndividualTest("first_test.py::test_one"),
                        Pass = true,
                        ModuleCoverages = new List<ModuleCoverage>()
                        {
                            new ModuleCoverage()
                            {
                                ModuleName = new ModuleName()
                                {
                                    Name = "first_test.py"
                                },
                                Contents = new[] {""}
                            }
                        }
                    },
                    new TestCoverage()
                    {
                        Test = new IndividualTest("first_test.py::test_two"),
                        Pass = false,
                        ModuleCoverages = new List<ModuleCoverage>()
                        {
                            new ModuleCoverage()
                            {
                                ModuleName = new ModuleName()
                                {
                                    Name = "first_test.py"
                                },
                                Contents = new[] {""}
                            }
                        }
                    },
                    new TestCoverage()
                    {
                        Test = new IndividualTest("second_test.py::test_three"),
                        Pass = true,
                        ModuleCoverages = new List<ModuleCoverage>()
                        {
                            new ModuleCoverage()
                            {
                                ModuleName = new ModuleName()
                                {
                                    Name = "second_test.py",
                                },Contents = new [] {""}
                            }
                        }
                    }
                }
            };

            Assert.IsTrue(CompareTraceTest(expected, actual));
        }

        private bool CompareTraceTest(TraceTests expected, TraceTests actual)
        {
            if (expected.TestCoverages.Count != actual.TestCoverages.Count) return false;

            for (int i = 0; i < expected.TestCoverages.Count; i++)
            {
                var expectedCoverage = expected.TestCoverages[i];
                var actualCoverage = actual.TestCoverages[i];

                if (expectedCoverage.ModuleCoverages.Count != actualCoverage.ModuleCoverages.Count)
                    return false;

                if (!Equals(expectedCoverage.Test, actualCoverage.Test)) return false;

                if (expectedCoverage.Pass != actualCoverage.Pass) return false;

                for (int j = 0; j < expectedCoverage.ModuleCoverages.Count; j++)
                {
                    var expectedModule = expectedCoverage.ModuleCoverages[j];
                    var actualModule = actualCoverage.ModuleCoverages[j];

                    if (expectedModule.ModuleName != actualModule.ModuleName) return false;
                }
            }

            return true;
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(CurrentDirectory, true);
        }
    }
}
