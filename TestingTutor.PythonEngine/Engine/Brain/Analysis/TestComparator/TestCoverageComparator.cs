using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TestingTutor.EngineModels;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.Parsers;
using TestingTutor.PythonEngine.Engine.Brain.Analysis.Trace;

namespace TestingTutor.PythonEngine.Engine.Brain.Analysis.TestComparator
{
    public class TestCoverageComparator : ITestCoverageComparator
    {
        public void Compare(ref IList<AnnotatedTest> annotatedTests, ref TraceTests instructor, ref TraceTests student, ref FeedbackDto feedback)
        {
            foreach (var coverage in instructor.TestCoverages)
            {
                var annotation = FindAnnotation(ref annotatedTests, coverage);
                var studentTests = GetStudentTests(coverage, ref student);
                TestStatusEnum testStatus;
                if (!coverage.Pass)
                {
                    testStatus = TestStatusEnum.Failed;
                }
                else
                {
                    var studentCoverage = FindCoverage(coverage, ref student);
                    testStatus = studentCoverage == null ? TestStatusEnum.Uncovered : TestStatusEnum.Covered;
                }

                feedback.InstructorTests.Add(
                    new InstructorTestDto
                    {
                        Name = coverage.Test.TestName,
                        Concepts = (annotation == null) ? new string[] { } : annotation.Concepts.ToArray(),
                        EquivalenceClass = (annotation == null) ? "NONE" : annotation.EquivalanceClass,
                        TestStatus = testStatus,
                        StudentTests = studentTests
                    });
            }
        }

        public IList<StudentTestDto> GetStudentTests(TestCoverage instructor, ref TraceTests student)
        {
            var studentTests = new List<StudentTestDto>();
            var testStatus = TestStatusEnum.Covered;

            foreach (var coverage in student.TestCoverages)
            {
                if (CompareModuleCoverages(instructor.ModuleCoverages, coverage.ModuleCoverages))
                {
                    studentTests.Add(
                        new StudentTestDto()
                        {
                            TestStatus = testStatus,
                            Name = coverage.Test.TestName,
                            Passed = coverage.Pass
                        });
                    testStatus = TestStatusEnum.Redundant;
                }
            }
            return studentTests;
        }


        

        public TestCoverage FindCoverage(TestCoverage coverage, ref TraceTests against)
        {
            foreach (var testCoverage in against.TestCoverages)
            {
                if (testCoverage.Pass != coverage.Pass)
                {
                    continue;
                }

                if (CompareModuleCoverages(testCoverage.ModuleCoverages,
                    coverage.ModuleCoverages))
                {
                    return testCoverage;
                }
            }
            return null;
        }

        public bool CompareModuleCoverages(IList<ModuleCoverage> left, IList<ModuleCoverage> right)
        {
            if (left.Count != right.Count)
                return false;

            var first = left.Except(right, new ModuleCoverageComparator()).ToList();
            var second = right.Except(left, new ModuleCoverageComparator()).ToList();

            return !first.Any() && !second.Any();
        }

        public class ModuleCoverageComparator : IEqualityComparer<ModuleCoverage>
        {
            public Regex CoverRegex = new Regex(@"^([\s]*[\d]+:)");

            public bool Equals(ModuleCoverage lhs, ModuleCoverage rhs)
            {
                if (lhs.ModuleName != rhs.ModuleName)
                    return false;

                if (lhs.Contents.Length != rhs.Contents.Length)
                    return false;

                for (int i = 0; i < lhs.Contents.Length; i++)
                {
                    var left = lhs.Contents[i];
                    var right = rhs.Contents[i];

                    if (CoverRegex.IsMatch(left) != CoverRegex.IsMatch(right))
                        return false;

                }
                return true;
            }

            public int GetHashCode(ModuleCoverage obj)
            {
                return 0;
            }
        }

        public AnnotatedTest FindAnnotation(ref IList<AnnotatedTest> annotations, TestCoverage coverage)
        {
            foreach (var annotation in annotations)
            {
                if (Equals(coverage.Test, annotation.IndividualTest))
                    return annotation;
            }

            return null;
        }
    }
}
