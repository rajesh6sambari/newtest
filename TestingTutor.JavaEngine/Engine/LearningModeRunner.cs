using System;
using System.IO;
using System.Linq;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Engine.Interfaces;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine
{
    public class LearningModeRunner : Mode, ILearningMode
    {
        public LearningModeRunner(Submission submission) : base(submission) { }

        public void Phase0Preparation()
        {
            try
            {
                Unpack(Submission.ReferenceSolution, Submission.ReferenceTestSolution,
                    out ReferenceCompilationUnit, WorkingDirectories.ReferenceOriginalCodeDirectory);
            }
            catch (EngineExceptionDto e)
            {
                e.Phase = "Phase 0";
                e.From = "Instructor";
                throw;
            }

            try
            {
                Unpack(Submission.ReferenceSolution, Submission.TestCaseSolution,
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

        public void Phase1Preprocessing()
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

        public void Phase2StudentInteraction(out FeedbackDto feedbackDto)
        {
            feedbackDto = new FeedbackDto
            {
                StudentId = Submission.SubmitterId,
                SubmissionId = Submission.SubmissionId
            };

            try
            {
                Compile(WorkingDirectories.StudentCodeDirectory, StudentCompilationUnit.SourceFiles);

                StudentJavaTestClasses = StudentCompilationUnit.Classes.ToList();

                Reflect(WorkingDirectories.StudentOriginalCodeDirectory, WorkingDirectories.StudentReflectionDirectory,
                    ref StudentJavaTestClasses);

                RunJunitTests(StudentJavaTestClasses,
                    WorkingDirectories.StudentOriginalCodeDirectory,
                    WorkingDirectories.StudentTraceDirectory, WorkingDirectories.ReferenceOriginalCodeDirectory);

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
    }
}