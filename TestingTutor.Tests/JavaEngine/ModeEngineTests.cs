using TestingTutor.JavaEngine.Engine;
using TestingTutor.JavaEngine.Models;
using Xunit;
using Submission = TestingTutor.JavaEngine.Models.Submission;

namespace TestingTutor.Tests.JavaEngine
{
    public class ModeEngineTests : TestBase
    {
        [Fact]
        public void LearningMode_Phase0Test_ShouldUnpackCode()
        {
            var mode = new LearningModeRunner(Submission.MapFrom(LearningModeSubmissionDto));
            mode.Phase0Preparation();
        }

        [Fact]
        public void LearningMode_Phase1Test_ShouldPass()
        {
            var mode = new LearningModeRunner(Submission.MapFrom(LearningModeSubmissionDto));
            mode.Phase0Preparation();
            mode.Phase1Preprocessing();
        }

        [Fact]
        public void LearningMode_Phase2Test_ShouldPass()
        {
            var mode = new LearningModeRunner(Submission.MapFrom(LearningModeSubmissionDto));
            mode.Phase0Preparation();
            mode.Phase1Preprocessing();
            mode.Phase2StudentInteraction(out var feedbackDto);
        }

        [Fact]
        public void DevelopmentMode_Phase0Test_ShouldUnpackCode()
        {
            var mode = new LearningModeRunner(Submission.MapFrom(DevelopmentModeSubmissionDto));
            mode.Phase0Preparation();
        }

        [Fact]
        public void DevelopmentMode_Phase1Test_ShouldPass()
        {
            var mode = new DevelopmentModeRunner(Submission.MapFrom(DevelopmentModeSubmissionDto));
            mode.Phase0Preparation();
            mode.Phase1ExecutionOfReferenceTestCases();
        }

        [Fact]
        public void DevelopmentMode_Phase2Test_ShouldPass()
        {
            var mode = new DevelopmentModeRunner(Submission.MapFrom(DevelopmentModeSubmissionDto));
            mode.Phase0Preparation();
            mode.Phase1ExecutionOfReferenceTestCases();
            mode.Phase2ExecutionOfStudentTestCases(out var feedbackDto);
        }
    }
}
