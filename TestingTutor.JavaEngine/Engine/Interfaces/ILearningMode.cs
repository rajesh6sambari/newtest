using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine.Interfaces
{
    public interface ILearningMode : IMode
    {
        void Phase1Preprocessing();
        void Phase2StudentInteraction(out FeedbackDto feedbackDto);
    }
}