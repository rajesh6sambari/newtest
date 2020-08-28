using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Engine.Interfaces;
using TestingTutor.JavaEngine.Models;

namespace TestingTutor.JavaEngine.Engine
{
    public class LearningModeEngine : IModeEngine
    {
        private readonly ILearningMode _mode;

        public LearningModeEngine(ILearningMode mode) => _mode = mode;

        public void Run(out FeedbackDto feedbackDto)
        {
            try
            {
                _mode.Phase0Preparation();
                _mode.Phase1Preprocessing();
                _mode.Phase2StudentInteraction(out feedbackDto);
            }
            catch (EngineExceptionDto engineException)
            {
                feedbackDto = new FeedbackDto()
                {
                    EngineExceptionDto = engineException,
                };
            }
            _mode.Phase3Cleanup();
        }
    }
}