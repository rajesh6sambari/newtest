using TestingTutor.JavaEngine.Engine.Interfaces;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine
{
    public static class ModeEngineFactory
    {
        public static IModeEngine GetEngine(Submission submission)
        {
            switch (submission.ApplicationMode)
            {
                case "Learning Mode":
                    return new LearningModeEngine(new LearningModeRunner(submission));
                case "Development Mode":
                    return new DevelopmentModeEngine(new DevelopmentModeRunner(submission));
                default:
                    return null;
            }
        }
    }
}
