using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine.Interfaces
{
    public interface IModeEngine
    {
        void Run(out FeedbackDto feedbackDto);
    }
}
