using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Engine.Interfaces;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine
{
    public class DevelopmentModeRunner : Mode, IDevelopmentMode
    {
        public DevelopmentModeRunner(Submission submission) : base(submission) { }

        public void Phase0Preparation()
        {
            try
            {
                Unpack(Submission.AssignmentSolution, Submission.ReferenceTestSolution,
                    out ReferenceCompilationUnit, WorkingDirectories.ReferenceOriginalCodeDirectory);

                Unpack(Submission.AssignmentSolution, Submission.TestCaseSolution,
                    out StudentCompilationUnit, WorkingDirectories.StudentOriginalCodeDirectory);
            }
            catch (EngineExceptionDto e)
            {
                e.Phase = "Phase 0";
                e.From = "Student";
                throw;
            }
        }

        public void Phase3Cleanup()
        {
            Directory.Delete(WorkingDirectories.ParentDirectory, true);
        }

        public void Phase1ExecutionOfReferenceTestCases()
        {
            try
            {
                Compile(WorkingDirectories.ReferenceOriginalCodeDirectory, ReferenceCompilationUnit.SourceFiles);

                ReferenceJavaTestClasses = ReferenceCompilationUnit.Classes.ToList();

                Reflect(WorkingDirectories.ReferenceOriginalCodeDirectory,
                    WorkingDirectories.ReferenceReflectionDirectory,
                    ref ReferenceJavaTestClasses);

                RunJunitTests(ReferenceJavaTestClasses,
                    WorkingDirectories.ReferenceOriginalCodeDirectory, WorkingDirectories.ReferenceTraceDirectory);

                RunTrace(WorkingDirectories.ReferenceTraceDirectory, ref ReferenceJavaTestClasses);

                Extract(WorkingDirectories.ReferenceTraceDirectory, ref ReferenceJavaTestClasses);
            }
            catch (EngineExceptionDto e)
            {
                e.Phase = "Phase 1";
                e.From = "Instructor";
                throw;
            }
        }

        public void Phase2ExecutionOfStudentTestCases(out FeedbackDto feedbackDto)
        {
            feedbackDto = new FeedbackDto();

            try
            {
                Compile(WorkingDirectories.StudentCodeDirectory, StudentCompilationUnit.SourceFiles);

                StudentJavaTestClasses = StudentCompilationUnit.Classes.ToList();

                Reflect(WorkingDirectories.StudentOriginalCodeDirectory, WorkingDirectories.StudentReflectionDirectory,
                    ref StudentJavaTestClasses);

                RunJunitTests(StudentJavaTestClasses,
                    WorkingDirectories.StudentOriginalCodeDirectory,
                    WorkingDirectories.StudentTraceDirectory, WorkingDirectories.ReferenceOriginalCodeDirectory);

                ValidateStudentJavaTestClasses(StudentJavaTestClasses);

                RunTrace(WorkingDirectories.StudentTraceDirectory, ref StudentJavaTestClasses);

                Extract(WorkingDirectories.StudentTraceDirectory, ref StudentJavaTestClasses);

                Analyze(ReferenceJavaTestClasses, StudentJavaTestClasses, out feedbackDto);

                RawCoverage(WorkingDirectories.StudentOriginalCodeDirectory, StudentJavaTestClasses, out var classCoverageDtos);
                feedbackDto.ClassCoveragesDto = classCoverageDtos;
            }
            catch (EngineExceptionDto e)
            {
                e.Phase = "Phase 2";
                e.From = "Student";
                throw;
            }
        }

        public void ValidateStudentJavaTestClasses(List<JavaTestClass> studentJavaTestClasses)
        {
            bool failed = false;
            var failedJavaTestClass = new List<JavaTestClass>();

            studentJavaTestClasses.ForEach(x =>
            {
                var javaClass = new JavaTestClass()
                {
                    Name = x.Name,
                    Package = x.Package,
                    PackageDirectory = x.PackageDirectory,
                };
                x.Methods.ToList().ForEach(y =>
                {
                    if (!y.Passed)
                    {
                        javaClass.AddMethod(y);
                        failed = true;
                    }
                });
                failedJavaTestClass.Add(javaClass);
            });

            if (failed)
            {
                var report = failedJavaTestClass.Where(x => x.Methods.Any()).Select(x =>
                {
                    var methods = $"{x.Package}.{x.Name}:\n{x.Methods.Select(y => y.Name).Join("\n")}";
                    return methods;
                }).Join("\n");

                throw new EngineExceptionDto()
                {
                    Report = "Failed Tests. In Development Mode all your test must passed.\n" +
                             "The following tests failed:\n" +
                             $"{report}",

                };
            }
        }
    }
}