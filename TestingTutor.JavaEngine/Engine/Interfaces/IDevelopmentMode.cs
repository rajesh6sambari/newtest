using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine.Interfaces
{
    public interface IDevelopmentMode : IMode
    {
        void Phase1ExecutionOfReferenceTestCases();
        void Phase2ExecutionOfStudentTestCases(out FeedbackDto feedbackDto);
    }
}