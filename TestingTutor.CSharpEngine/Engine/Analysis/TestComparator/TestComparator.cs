using System;
using System.Collections.Generic;
using System.Linq;
using TestingTutor.CSharpEngine.Engine.Analysis.Parser;
using TestingTutor.CSharpEngine.Engine.Analysis.Trace;
using TestingTutor.EngineModels;
using OpenCover.Framework.Model;
using TestingTutor.CSharpEngine.OpenCover;
using System.Xml.Serialization;
using System.IO;
using TestingTutor.CSharpEngine.Models;


namespace TestingTutor.CSharpEngine.Engine.Analysis.TestComparator
{
    public class TestCoverageComparator : ITestCoverageComparator
    {
        private string studentDirectory;
        private string instructorDirectory;

        public void Compare(ref IList<AnnotatedTest> annotatedTests, ref TraceTests instructor, 
                ref TraceTests student, ref FeedbackDto feedback, EngineWorkingDirectories workingDirectories)
        {
            studentDirectory = workingDirectories.StudentDllPath;
            instructorDirectory = workingDirectories.InstructorDllPath;

            var InstructorMethodData = GetMethodMetaDatas(instructor.TestCoverages);
            var instCov = GetInstructorCoverage(InstructorMethodData, ref instructor);
            var StudentMethodData = GetMethodMetaDatas(student.TestCoverages);

            foreach (var coverage in instructor.TestCoverages)
            {
                var annotation = FindAnnotation(ref annotatedTests, coverage);
               
                var studentTests = new List<StudentTestDto>();

                TestStatusEnum testStatus = TestStatusEnum.Redundant;
                if (!coverage.Pass)
                {
                    testStatus = TestStatusEnum.Failed;
                }
                else
                {
                    var testData =  StudentMethodData
                                    .SelectMany(x => x
                                    .Where(y => y.TestName == coverage.Test.TestName));
                   
                    
                    var studentCoverage = FindCoverage(testData);
                    int count = 0;

                    foreach (var test in instCov)
                    {
                        if (test.Item1.Count() == studentCoverage.Count())
                        {
                            var areEq = test.Item1.SequenceEqual(studentCoverage);


                            var instDto = feedback.InstructorTests.FirstOrDefault(x => x.Name == test.Item2);

                            if (instDto != null)
                            {
                                if (instDto.TestStatus == TestStatusEnum.Covered)
                                {
                                    studentTests.Add(
                                      new StudentTestDto()
                                      {
                                          TestStatus = TestStatusEnum.Redundant,
                                          Name = coverage.Test.TestName,
                                          Passed = coverage.Pass
                                      });
                                    testStatus = TestStatusEnum.Redundant;
                                }
                            }
                            else if (areEq && count == 0)
                            {
                                studentTests.Add(
                                   new StudentTestDto()
                                   {
                                       TestStatus = TestStatusEnum.Covered,
                                       Name = coverage.Test.TestName,
                                       Passed = coverage.Pass
                                   });
                                count++;
                                testStatus = TestStatusEnum.Covered;
                            }
                            else if(!areEq)
                            {
                                studentTests.Add(
                                        new StudentTestDto()
                                        {
                                            TestStatus = TestStatusEnum.Uncovered,
                                            Name = coverage.Test.TestName,
                                            Passed = coverage.Pass
                                        });
                                testStatus = TestStatusEnum.Uncovered;
                            }
                        }
                    }
                }

                foreach(var test in feedback.InstructorTests)
                {
                    if(test.StudentTests.Count() == 0)
                    {
                        test.TestStatus = TestStatusEnum.Uncovered;
                    }

                    if(test.StudentTests.Count() > 1)
                    {
                        test.TestStatus = TestStatusEnum.Redundant;
                        var t = test.StudentTests.ToArray().Where(x => x.Name == x.Name); 
                        if(t.Count() == test.StudentTests.Count())
                        {
                            test.TestStatus = TestStatusEnum.Covered;
                        }
                    }
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

            var coverageSession = GetTotalCoverage(workingDirectories);

            //OpenCover generates reports for the methods being tested themselves, so it will show double the data it should
            //on coverage percentage, and statements. It is possible this can be filtered out in OpenCover, but I am not sure
            //how to do it without completel excluding the actual coverage data
            feedback.NumberOfBranchesHit = coverageSession.Summary.VisitedBranchPoints;
            feedback.NumberOfMissingStatements = (coverageSession.Summary.NumSequencePoints/2) - coverageSession.Summary.VisitedSequencePoints;
            feedback.NumberOfStatements = coverageSession.Summary.NumSequencePoints/2;
            feedback.CoveragePercentage = (double)coverageSession.Summary.BranchCoverage * 2;
        }

        public CoverageSession GetTotalCoverage(EngineWorkingDirectories workingDirectories)
        {

            var dir = Directory.GetCurrentDirectory();
            var path = Directory.CreateDirectory(dir + "\\results");

            OpenTrace openTrace = new OpenTrace(dir);
            var testPath = Path.Combine(dir + "\\results" + "\\TotalCoverage.xml");
            var file = System.IO.File.Create(testPath);
            file.Close();

            var name = workingDirectories.StudentDllPath.Split('\\');

            openTrace.Run(null, path.FullName, "results", workingDirectories.StudentDllPath+"\\bin\\Debug\\"+ name.LastOrDefault() +".dll");

            XmlSerializer serializer = new XmlSerializer(typeof(CoverageSession));
            FileStream fs = new FileStream(path.FullName + "\\results.xml", FileMode.Open);

            CoverageSession coverageSession;

            coverageSession = (CoverageSession)serializer.Deserialize(fs);

            fs.Close();

            Directory.Delete(path.FullName, true);

            return coverageSession;
        }

        public List<IEnumerable<MethodMetaData>> GetMethodMetaDatas(IList<TestCoverage> coverage)
        {
            List<IEnumerable<MethodMetaData>> methodMetaDatas = new List<IEnumerable<MethodMetaData>>();
          

            foreach (var cov in coverage)
            {
                methodMetaDatas.Add(GetTestCoverageMetrics(cov));
            }

            return methodMetaDatas;
        }

        public List<int> FindCoverage(IEnumerable<MethodMetaData> MethodMetaDatas)
        {
            List<int> linesCovered = new List<int>();

            foreach(var methodData in MethodMetaDatas)
            {
                var line = methodData.CoveredLineSections
                    .Select(y => y.Item1);

                foreach (var l in line)
                {
                    if (!linesCovered.Contains(l))
                    {
                        linesCovered.Add(l);
                    }
                }
   
            }
            return linesCovered;
        }

        public List<Tuple<List<int>, string>> GetInstructorCoverage(List<IEnumerable<MethodMetaData>> methodMetaDatas, ref TraceTests instructor)
        {
            List<Tuple<List<int>, string>> list = new List<Tuple<List<int>, string>>();

            foreach (var method in methodMetaDatas)
             {
                foreach (var i in instructor.TestCoverages)
                {
                    var m = method.Where(x => x.TestName == i.Test.TestName);

                    if (m != null)
                    {
                        var returnVal = FindCoverage(m);

                        var names = list
                            .Select(x => x.Item2);

                        if (!names.Contains(i.Test.TestName) && returnVal.Count() !=0)
                        {
                            list.Add(new Tuple<List<int>, string>(returnVal, i.Test.TestName));
                        }
                    }
                }
            }
            return list;
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

        public IEnumerable<MethodMetaData> GetTestCoverageMetrics(TestCoverage coverage)
        {
            var visitedMethodMetadata = coverage.CoverageSession.Modules
                .SelectMany(c => c.Classes) //Flat list of all classes in coverage session
                .SelectMany(f => f.Methods) //Flat list of all the functions touched in the coverage session
                .Where(f => f.Visited) //Filter out any function that weren't visited.
                .Select(f => new MethodMetaData(f, coverage.Test.TestName)); //Turn all of the methods into my custom meta data class. Look inside for details.
           
            return visitedMethodMetadata;
        }

        public class MethodMetaData
        {
            public MethodMetaData(Method method, string testName)
            {
                MethodInfo = method;
                TestName = testName;
            }

            private Method MethodInfo { get; }

            public int NumLinesCovered
            {
                get
                {
                    //Ensure that no lines are counted twice.
                    return CoveredLineSections
                        .SelectMany(c => Enumerable.Range(c.Item1, c.Item2 - c.Item1)) //Gets a list of line numbers for each sequence point -- all together
                        .Distinct() //Weeds out any duplicate line numbers
                        .Count(); //How many lines were covered.
                }
            }

            public string MethodName => MethodInfo.FullName;
            public string TestName { get; set; }

            public List<Tuple<int, int>> CoveredLineSections
            {
                get
                {
                    var lineSections = new List<Tuple<int, int>>();
                    foreach (var seqPt in MethodInfo.SequencePoints)
                    {
                        //Add each covered line block to a list. Each block stored in a tuple for convenience.
                        lineSections.Add(new Tuple<int, int>(seqPt.StartLine, seqPt.EndLine));
                    }

                    return lineSections;
                }
            }
        }
    }
}
