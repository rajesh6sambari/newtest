using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingTutor.EngineModels;
using TestingTutor.PythonEngine.Engine;
using TestingTutor.PythonEngine.Engine.Brain.Analysis;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.TestComparator;
using TestingTutor.PythonEngine.Engine.Brain.CoverageStat;
using TestingTutor.PythonEngine.Engine.Brain.Feedback;
using TestingTutor.PythonEngine.Engine.Brain.Preprocessing;
using TestingTutor.PythonEngine.Engine.Factory;
using TestingTutor.PythonEngine.Engine.Utilities;
using TestingTutor.PythonEngine.Engine.Utilities.FileHandlers;
using TestingTutor.PythonEngine.Engine.Utilities.Python;
using TestingTutor.PythonEngine.Engine.Utilities.Workspaces;

namespace TestingTutor.PythonEngine.Tests.EngineTests
{
    [TestClass]
    public class FullEngineTests
    {
        protected IEngine Engine;
        protected IEngineFactory Factory;
        protected string CurrentDirectory;

        [TestInitialize]
        public void Init()
        {
            CurrentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "FullEngineTestDir");
            Directory.CreateDirectory(CurrentDirectory);
        }

        [TestMethod]
        public void FullEngineLearningModeShouldPassWithOneTest()
        {
            // Arrange
            FeedbackDto actual = new FeedbackDto();
            Factory = new MockEngineFactory(CurrentDirectory,
                new MockFeedback(f => actual = f));
            Engine = new Engine.PythonEngine(Factory);
            var submission = new SubmissionDto()
            {
                ApplicationMode = "Learning Mode",
                SubmitterId = "Random",
                SubmissionId = 1,
                TestCaseSolution = ConvertToZipByteArray(
                    "import sys\r\n" +
                    "sys.path.append('../Solution/')\r\n" +
                    "from solution import func\r\n" +
                    "def test():\r\n" +
                    "   assert func() == 3",
                    "test_student.py"),
                ReferenceSolution = ConvertToZipByteArray(
                            "def func():\r\n" +
                            "   return 3",
                            "solution.py"),
                SolutionFolderName = "Solution",
                ReferenceTestSolution = ConvertToZipByteArray(
                            "import sys\r\n" +
                            "sys.path.append('../Solution/')\r\n" +
                            "from solution import func\r\n" +
                            "def test_teacher(record_property):\r\n" +
                            "   record_property(\"EquivalanceClass\", \"FUNCTION CALL\")\r\n" +
                            "   record_property(\"Concepts\", \"func,equals\")\r\n" +
                            "   assert func() == 3\r\n"
                            , "test_teacher.py"),
            };

            // Act
            var feedbackDto =  Engine.Run(submission);

            // Assert
            var expected = new FeedbackDto()
            {
                InstructorTests = new List<InstructorTestDto>()
                {
                    new InstructorTestDto()
                    {
                        Name = "test_teacher.py::test_teacher",
                        Concepts = new string[] {"func", "equals"},
                        EquivalenceClass = "FUNCTION CALL",
                        TestStatus = TestStatusEnum.Covered,
                        StudentTests = new List<StudentTestDto>()
                        {
                            new StudentTestDto()
                            {
                                Name = "test_student.py::test",
                                Passed = true,
                                TestStatus = TestStatusEnum.Covered
                            }
                        }
                    }
                },
                CoveragePercentage = 1,
                NumberOfStatements = 2,
                NumberOfMissingStatements = 0,
                NumberOfBranchesHit = 0,
                SubmissionId = 1,
                StudentId = "Random"
            };
            Assert.IsTrue(CompareFeedback(expected, actual));
        }


        [TestMethod]
        public void FullEngineLearningModeShouldPassWithMultipleTest()
        {
            // Arrange
            FeedbackDto actual = new FeedbackDto();
            Factory = new MockEngineFactory(CurrentDirectory,
                new MockFeedback(f => actual = f));
            Engine = new Engine.PythonEngine(Factory);
            var submission = new SubmissionDto()
            {
                ApplicationMode = "Learning Mode",
                SubmissionId = 1,
                TestCaseSolution = ConvertToZipByteArray(
                    "import sys\r\n" +
                    "sys.path.append('../Solution/')\r\n" +
                    "from solution import Calc\r\n" +
                    "def test_add():\r\n" +
                    "   assert Calc().add(5,5) == 10\r\n" +
                    "def test_add2():\r\n" +
                    "   assert Calc().add(1,2) == 3\r\n" +
                    "def test_two():\r\n" +
                    "   assert Calc().subt(4,3) == 1\r\n",
                    "test_student.py"),
                ReferenceSolution = ConvertToZipByteArray(
                            "class Calc():\r\n" +
                            "   def add(self,x,y):\r\n" +
                            "       return x + y\r\n" +
                            "   def subt(self,x,y):\r\n" +
                            "       return x - y\r\n" +
                            "   def mult(self,x,y):\r\n" +
                            "       return x * y",
                            "solution.py"),
                SolutionFolderName = "Solution",
                ReferenceTestSolution = ConvertToZipByteArray(
                    "import sys\r\n" +
                    "sys.path.append('../Solution/')\r\n" +
                    "from solution import Calc\r\n" +
                    "class Test():\r\n" +
                    "   calculator = Calc()\r\n" +
                    "   def test_one(self,record_property):\r\n" +
                    "       record_property(\"EquivalanceClass\", \"ADDITION\")\r\n" +
                    "       record_property(\"Concepts\", \"add,pos\")\r\n" +
                    "       assert self.calculator.add(1,2) == 3\r\n" +
                    "   def test_two(self,record_property):\r\n" +
                    "       record_property(\"EquivalanceClass\", \"SUBTRACTION\")\r\n" +
                    "       record_property(\"Concepts\", \"subt,pos\")\r\n" +
                    "       assert self.calculator.subt(4,3) == 1\r\n" +
                    "   def test_three(self, record_property):\r\n" +
                    "       record_property(\"EquivalanceClass\", \"MULTIPLICATION\")\r\n" +
                    "       record_property(\"Concepts\", \"mult,pos\")\r\n" +
                    "       assert self.calculator.mult(3,3) == 9\r\n",
                    "test_teacher.py"),
            };

            // Act
            var feedbackDto = Engine.Run(submission);

            // Assert
            var expected = new FeedbackDto()
            {
                InstructorTests = new List<InstructorTestDto>()
                {
                    new InstructorTestDto()
                    {
                        Name = "test_teacher.py::Test::test_one",
                        Concepts = new []{"add", "pos"},
                        EquivalenceClass = "ADDITION",
                        TestStatus = TestStatusEnum.Covered,
                        StudentTests = new List<StudentTestDto>()
                        {
                            new StudentTestDto()
                            {
                                Name = "test_student.py::test_add",
                                Passed = true,
                                TestStatus = TestStatusEnum.Covered,
                            },
                            new StudentTestDto()
                            {
                                Name = "test_student.py::test_add2",
                                Passed = true,
                                TestStatus = TestStatusEnum.Redundant
                            },
                        }
                    },
                    new InstructorTestDto()
                    {
                        Concepts = new [] {"subt", "pos"},
                        EquivalenceClass = "SUBTRACTION",
                        Name = "test_teacher.py::Test::test_two",
                        TestStatus = TestStatusEnum.Covered,
                        StudentTests = new List<StudentTestDto>()
                        {
                            new StudentTestDto()
                            {
                                Name = "test_student.py::test_two",
                                TestStatus = TestStatusEnum.Covered,
                                Passed = true,
                            }
                        }
                    },
                    new InstructorTestDto()
                    {
                        Concepts = new [] {"mult", "pos"},
                        EquivalenceClass = "MULTIPLICATION",
                        Name = "test_teacher.py::Test::test_three",
                        TestStatus = TestStatusEnum.Uncovered,
                        StudentTests = new List<StudentTestDto>()
                    },
                },
                SubmissionId = 1,
                CoveragePercentage = 0.86,
                NumberOfStatements = 7,
                NumberOfMissingStatements = 1,
                NumberOfBranchesHit = 0
            };
            Assert.IsTrue(CompareFeedback(expected, actual));
        }

        [TestMethod]
        public void FullEngineDevelopingModeShouldPassWithMultipleTest()
        {
            // Arrange
            FeedbackDto actual = new FeedbackDto();
            Factory = new MockEngineFactory(CurrentDirectory,
                new MockFeedback(f => actual = f));
            Engine = new Engine.PythonEngine(Factory);
            var submission = new SubmissionDto()
            {
                ApplicationMode = "Development Mode",
                SubmissionId = 1,
                AssignmentSolution = ConvertToZipByteArray(
                        "class Calc():\r\n" +
                        "   def add(self,x,y):\r\n" +
                        "       return x + y\r\n" +
                        "   def subt(self,x,y):\r\n" +
                        "       return x + y\r\n" +
                        "   def mult(self,x,y):\r\n" +
                        "       return x * y\r\n" +
                        "   def div(self,x,y):\r\n" +
                        "       return x / y",
                        "solution.py"),
                TestCaseSolution = ConvertToZipByteArray(
                    "import sys\r\n" +
                    "sys.path.append('../Solution/')\r\n" +
                    "from solution import Calc\r\n" +
                    "def test_add():\r\n" +
                    "   assert Calc().add(5,5) == 10\r\n" +
                    "def test_mult():\r\n" +
                    "   assert Calc().mult(1,2) == 2\r\n" +
                    "def test_two():\r\n" +
                    "   assert Calc().add(4,3) == 7\r\n" +
                    "\r\n" +
                    "def test_div():\r\n" +
                    "   assert Calc().div(8,4) == 2\r\n",
                    "test_student.py"),
                ReferenceSolution = ConvertToZipByteArray(
                            "class Calc():\r\n" +
                            "   def add(self,x,y):\r\n" +
                            "       return x + y\r\n" +
                            "   def subt(self,x,y):\r\n" +
                            "       return x - y\r\n" +
                            "   def mult(self,x,y):\r\n" +
                            "       return x * y",
                            "solution.py"),
                SolutionFolderName = "Solution",
                ReferenceTestSolution = ConvertToZipByteArray(
                    "import sys\r\n" +
                    "sys.path.append('../Solution/')\r\n" +
                    "from solution import Calc\r\n" +
                    "class Test():\r\n" +
                    "   calculator = Calc()\r\n" +
                    "   def test_one(self,record_property):\r\n" +
                    "       record_property(\"EquivalanceClass\", \"ADDITION\")\r\n" +
                    "       record_property(\"Concepts\", \"add,pos\")\r\n" +
                    "       assert self.calculator.add(1,2) == 3\r\n" +
                    "   def test_two(self,record_property):\r\n" +
                    "       record_property(\"EquivalanceClass\", \"SUBTRACTION\")\r\n" +
                    "       record_property(\"Concepts\", \"subt,pos\")\r\n" +
                    "       assert self.calculator.subt(4,3) == 1\r\n" +
                    "   def test_three(self, record_property):\r\n" +
                    "       record_property(\"EquivalanceClass\", \"MULTIPLICATION\")\r\n" +
                    "       record_property(\"Concepts\", \"mul,pos\")\r\n" +
                    "       assert self.calculator.mult(3,3) == 9\r\n",
                    "test_teacher.py"),
            };

            // Act
            var feedbackDto = Engine.Run(submission);

            // Assert
            var expected = new FeedbackDto()
            {
                InstructorTests = new List<InstructorTestDto>()
                {
                    new InstructorTestDto()
                    {
                        Name = "test_teacher.py::Test::test_one",
                        Concepts = new []{"add", "pos"},
                        EquivalenceClass = "ADDITION",
                        StudentTests = new List<StudentTestDto>()
                        {
                            new StudentTestDto()
                            {
                                Name = "test_student.py::test_add",
                                Passed = true,
                                TestStatus = TestStatusEnum.Covered
                            },
                            new StudentTestDto()
                            {
                                Name = "test_student.py::test_two",
                                Passed = true,
                                TestStatus = TestStatusEnum.Redundant
                            }
                        }
                    },
                    new InstructorTestDto()
                    {
                        Name = "test_teacher.py::Test::test_two",
                        Concepts = new []{ "subt", "pos" },
                        EquivalenceClass = "SUBTRACTION",
                        TestStatus = TestStatusEnum.Failed,
                        StudentTests = new List<StudentTestDto>()
                    },
                    new InstructorTestDto()
                    {
                        Name = "test_teacher.py::Test::test_three",
                        Concepts = new [] {"mul", "pos"},
                        EquivalenceClass = "MULTIPLICATION",
                        TestStatus = TestStatusEnum.Covered,
                        StudentTests = new List<StudentTestDto>()
                        {
                            new StudentTestDto()
                            {
                                Name = "test_student.py::test_mult",
                                Passed = true,
                                TestStatus = TestStatusEnum.Covered
                            }
                        }
                    }
                },
                SubmissionId = 1,
                CoveragePercentage = .89,
                NumberOfStatements = 9,
                NumberOfMissingStatements = 1,
                NumberOfBranchesHit = 0
            };
            Assert.IsTrue(CompareFeedback(expected, actual));
        }



        public bool CompareFeedback(FeedbackDto lhs, FeedbackDto rhs)
        {

            if (Math.Abs(lhs.CoveragePercentage - rhs.CoveragePercentage) > 0.1)
                return false;

            if (lhs.NumberOfStatements != rhs.NumberOfStatements)
                return false;

            if (lhs.NumberOfBranchesHit != rhs.NumberOfBranchesHit)
                return false;

            if (lhs.NumberOfMissingStatements != rhs.NumberOfMissingStatements)
                return false;

            if (!CompareTestResult(lhs.InstructorTests, rhs.InstructorTests))
                return false;

            return true;
        }

        public bool CompareTestResult(IList<InstructorTestDto> left, IList<InstructorTestDto> right)
        {
            if (left.Count != right.Count) return false;

            var first = left.Except(right, new InstructorTestDtoComparator()).ToList();
            var second = right.Except(left, new InstructorTestDtoComparator()).ToList();

            return !first.Any() && !second.Any();
        }

        public class StudentTestDtoComparator : IEqualityComparer<StudentTestDto>
        {
            public bool Equals(StudentTestDto x, StudentTestDto y)
            {
                if (!x.Name.Equals(y.Name))
                    return false;

                if (x.TestStatus != y.TestStatus)
                    return false;

                if (x.Passed != y.Passed)
                    return false;

                return true;
            }

            public int GetHashCode(StudentTestDto obj)
            {
                return 0;
            }
        }

        public class InstructorTestDtoComparator : IEqualityComparer<InstructorTestDto>
        {
            public bool Equals(InstructorTestDto x, InstructorTestDto y)
            {
                if (!x.Name.Equals(y.Name))
                    return false;

                if (x.TestStatus != y.TestStatus)
                    return false;

                if (!x.EquivalenceClass.Equals(y.EquivalenceClass))
                    return false;

                if (x.Concepts.Length != y.Concepts.Length)
                    return false;

                var leftConcepts = x.Concepts.ToList();
                var rightConcepts = y.Concepts.ToList();

                var leftConceptsExcept = leftConcepts.Except(rightConcepts).ToList();
                var rightConceptsExcept = rightConcepts.Except(leftConcepts).ToList();

                if (leftConceptsExcept.Any() || rightConceptsExcept.Any())
                    return false;

                var leftStudentTest = x.StudentTests.ToList();
                var rightStudentTest = y.StudentTests.ToList();

                var leftStudentExcept = leftStudentTest.Except(rightStudentTest, new StudentTestDtoComparator()).ToList();
                var rightStudentExcept = rightStudentTest.Except(leftStudentTest, new StudentTestDtoComparator()).ToList();

                return !leftStudentExcept.Any() && !rightStudentExcept.Any();
            }

            public int GetHashCode(InstructorTestDto obj)
            {
                return 0;
            }
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
        public void Clean()
        {
            Directory.Delete(CurrentDirectory);
        }


        public class MockFeedback : IFeedbackSender
        {
            private readonly Action<FeedbackDto> _function;

            public MockFeedback(Action<FeedbackDto> function)
            {
                _function = function;
            }

            public void SendFeedback(FeedbackDto feedback)
            {
                _function(feedback);
            }
        }

        public class MockEngineFactory : IEngineFactory
        {
            protected string Root;
            protected const string PytestPath = @"C:\Pytest\pytest.py";
            private readonly IFeedbackSender _sender;
            public MockEngineFactory(string root, IFeedbackSender sender)
            {
                Root = root;
                _sender = sender;
            }

            public IWorkspace Workspace(string identifier)
            {
                return new TempWorkspace(Path.Combine(Root, identifier));
            }

            public IFileHandler FileHandler()
            {
                return new FileHandler();
            }

            public IPreprocessor Preprocessor(string mode)
            {
                switch (mode)
                {
                    case "Learning Mode":
                        return new LearningPreprocessor(this);
                    case "Development Mode":
                        return new DevelopingPreprocessor(this);
                }

                throw new ArgumentException("Mode doesn't exist");
            }

            public IPytest Pytest()
            {
                return new Pytest();
            }

            public IFeedbackSender FeedbackSender()
            {
                return _sender;
            }

            public ITestCaseAnalysis TestCaseAnalysis()
            {
                return new TestCaseAnalysis(this);
            }

            public ITrace Trace()
            {
                return new PyTracer(PytestPath);
            }

            public ITestCoverageComparator Comparator()
            {
                return new TestCoverageComparator();
            }

            public ICoverageStats CoverageStats()
            {
                return new CoverageStats(this);
            }

            public IPytestCov PytestCov()
            {
                return new PytestCov();
            }
        }
    }
}
