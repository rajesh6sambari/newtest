using System.Collections.Generic;
using System.Linq;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Engine.Interfaces;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine
{
    public class TraceAnalysis : ITraceAnalysis
    {
        public void Analyze(List<JavaTestClass> referenceTests, List<JavaTestClass> studentTests, out FeedbackDto feedbackDto)
        {
            feedbackDto = new FeedbackDto();

            foreach (var referenceTest in referenceTests)
            {
                foreach (var referenceMethod in referenceTest.Methods)
                {
                    if (referenceMethod.LearningConcepts != null)
                    {
                        var instructorTestDto = GetInstructorTestDto(referenceMethod, studentTests);
                        feedbackDto.InstructorTests.Add(instructorTestDto);
                    }
                }
            }
        }

        public InstructorTestDto GetInstructorTestDto(JavaTestMethod referenceTestMethod, List<JavaTestClass> studentTests)
        {
            var instructorTestDto = new InstructorTestDto
            {
                Name = referenceTestMethod.Name,
                EquivalenceClass = referenceTestMethod.EquivalenceClass,
                Concepts = referenceTestMethod.LearningConcepts,
                Passed = referenceTestMethod.Passed,
            };

            foreach (var studentTestClass in studentTests)
            {
                foreach (var studentMethod in studentTestClass.Methods)
                {
                    if (studentMethod.LearningConcepts == null)
                    {
                        CompareToStudentMethod(instructorTestDto, referenceTestMethod, studentMethod);
                    }
                }
            }

            MarkInstructorDto(instructorTestDto);

            return instructorTestDto;
        }

        public void CompareToStudentMethod(InstructorTestDto instructorTestDto,
            JavaTestMethod instructorTestMethod,
            JavaTestMethod studentTestMethod)
        {
            if (Covered(instructorTestMethod, studentTestMethod))
            {
                TestStatusEnum status;
                switch (instructorTestDto.StudentTests.Count)
                {
                    case 0:
                        status = TestStatusEnum.Uncovered;
                        break;
                    case 1:
                        status = TestStatusEnum.Covered;
                        break;
                    default:
                        {
                            status = TestStatusEnum.Redundant;
                            break;
                        }
                }
                instructorTestDto.StudentTests.Add(new StudentTestDto
                {
                    Name = studentTestMethod.Name,
                    Passed = studentTestMethod.Passed,
                    TestStatus = status
                });
            }
        }

        public void MarkInstructorDto(InstructorTestDto instructorTestDto)
        {
            switch (instructorTestDto.StudentTests.Count)
            {
                case 0:
                    instructorTestDto.TestStatus = TestStatusEnum.Uncovered;
                    break;
                case 1:
                    instructorTestDto.TestStatus = TestStatusEnum.Covered;
                    break;
                default:
                    {
                        instructorTestDto.TestStatus = TestStatusEnum.Redundant;
                        break;
                    }
            }
        }

        private static bool Covered(JavaTestMethod referenceTestMethod, JavaTestMethod studentTestMethod)
        {
            if (referenceTestMethod.LineCoverages.Count != studentTestMethod.LineCoverages.Count)
                throw new EngineExceptionDto()
                {
                    Report = JavaEngineReportExceptionFactory.GenerateReportForAnalysisLineCoverage(referenceTestMethod, studentTestMethod)
                };
            return !referenceTestMethod.LineCoverages.Where((r, i) => !(r.CoveredInstructions > 0
                ? studentTestMethod.LineCoverages[i].CoveredInstructions > 0
                : studentTestMethod.LineCoverages[i].CoveredInstructions == 0)).Any();
            //return !referenceTestMethod.LineCoverages.Where((t, i) => t.CoveredBranches > studentTestMethod.LineCoverages[i].CoveredBranches
            //                                                      || t.CoveredInstructions > studentTestMethod.LineCoverages[i].CoveredInstructions 
            //                                                      || t.MissedBranches < studentTestMethod.LineCoverages[i].MissedBranches 
            //                                                      || t.MissedInstructions < studentTestMethod.LineCoverages[i].MissedInstructions).Any();
        }
    }
}