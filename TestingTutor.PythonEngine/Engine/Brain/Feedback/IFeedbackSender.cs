using TestingTutor.EngineModels;

namespace TestingTutor.PythonEngine.Engine.Brain.Feedback
{
    public interface IFeedbackSender
    {
        void SendFeedback(FeedbackDto feedback);
    }
}
